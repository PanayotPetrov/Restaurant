namespace Restaurant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;

    public class OrderService : IOrderService
    {
        private readonly IDeletableEntityRepository<Order> orderRepository;
        private readonly ICartService cartService;

        public OrderService(IDeletableEntityRepository<Order> orderRepository, ICartService cartService)
        {
            this.orderRepository = orderRepository;
            this.cartService = cartService;
        }

        public T GetByUserIdAndOrderNumber<T>(string userId, string orderNumber)
        {
            return this.orderRepository.AllAsNoTrackingWithDeleted().Where(o => o.ApplicationUserId == userId && o.OrderNumber == orderNumber).To<T>().FirstOrDefault();
        }

        public IEnumerable<string> GetAllOrderNumbersByUserId(string userId)
        {
            return this.orderRepository.AllAsNoTrackingWithDeleted().Where(c => c.ApplicationUserId == userId).OrderBy(o => o.IsComplete).Select(o => o.OrderNumber).ToList();
        }

        public async Task<string> CreateAsync(AddOrderModel model)
        {
            var cart = this.cartService.GetCartById<Cart>(model.CartId);

            var order = AutoMapperConfig.MapperInstance.Map<Order>(model);
            order.OrderNumber = Guid.NewGuid().ToString().Substring(0, 8);
            order.TotalPrice = cart.TotalPrice;

            if (DateTime.UtcNow.Hour >= 20 || DateTime.UtcNow.Hour <= 8)
            {
                order.DeliveryTime = DateTime.UtcNow.Date.AddDays(1).AddHours(10);
            }
            else
            {
                order.DeliveryTime = DateTime.UtcNow.AddHours(1);
            }

            foreach (var cartItem in cart.CartItems)
            {
                order.Meals.Add(new MealOrder { MealId = cartItem.MealId, MealQuantity = cartItem.Quantity });
            }

            await this.orderRepository.AddAsync(order);
            await this.orderRepository.SaveChangesAsync();
            await this.cartService.ClearCartAsync(cart.Id);
            return order.OrderNumber;
        }
    }
}
