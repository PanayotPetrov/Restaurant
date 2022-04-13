namespace Restaurant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
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
            return this.orderRepository.AllAsNoTracking().IgnoreQueryFilters().Where(o => o.ApplicationUserId == userId && o.OrderNumber == orderNumber).To<T>().FirstOrDefault();
        }

        public IEnumerable<string> GetAllOrderNumbersByUserId(string userId)
        {
            return this.orderRepository.AllAsNoTracking().Where(c => c.ApplicationUserId == userId).OrderBy(o => o.IsComplete).Select(o => o.OrderNumber).ToList();
        }

        public IEnumerable<string> GetAllOrderNumbers()
        {
            return this.orderRepository.AllAsNoTrackingWithDeleted().Select(o => o.OrderNumber).ToList();
        }

        public IEnumerable<T> GetAllWithPagination<T>(int itemsPerPage, int page)
        {
            return this.orderRepository.AllAsNoTrackingWithDeleted().OrderByDescending(x => x.CreatedOn).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).To<T>().ToList();
        }

        public int GetCountWithDeleted()
        {
            return this.orderRepository.AllAsNoTrackingWithDeleted().Count();
        }

        public T GetByOrderNumberWithDeleted<T>(string orderNumber)
        {
            return this.orderRepository.AllAsNoTrackingWithDeleted().Where(o => o.OrderNumber == orderNumber).To<T>().FirstOrDefault();
        }

        public async Task<string> CreateAsync(AddOrderModel model)
        {
            var cart = this.cartService.GetCartById(model.CartId);

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

        public async Task<bool> DeleteByOrderNumberAsync(string orderNumber)
        {
            var order = this.orderRepository.AllWithDeleted().FirstOrDefault(o => o.OrderNumber == orderNumber);
            if (order.IsDeleted)
            {
                return false;
            }

            this.orderRepository.Delete(order);
            await this.orderRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreAsync(string orderNumber)
        {
            var order = this.orderRepository.AllWithDeleted().FirstOrDefault(o => o.OrderNumber == orderNumber);
            if (!order.IsDeleted)
            {
                return false;
            }

            order.IsDeleted = false;
            order.DeletedOn = null;
            await this.orderRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteAsync(string orderNumber)
        {
            var order = this.orderRepository.AllWithDeleted().FirstOrDefault(o => o.OrderNumber == orderNumber);
            if (order.IsComplete)
            {
                return false;
            }

            order.IsComplete = true;
            order.CompletedOn = DateTime.UtcNow;
            await this.orderRepository.SaveChangesAsync();
            return true;
        }
    }
}
