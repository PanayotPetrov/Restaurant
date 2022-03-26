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

        public OrderService(IDeletableEntityRepository<Order> orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<string> CreateAsync(AddOrderModel model)
        {
            var order = AutoMapperConfig.MapperInstance.Map<Order>(model);
            order.OrderNumber = Guid.NewGuid().ToString().Substring(0, 8);
            await this.orderRepository.AddAsync(order);
            await this.orderRepository.SaveChangesAsync();
            return order.OrderNumber;
        }

        public IEnumerable<T> GetAllByUserId<T>(string userId)
        {
            return this.orderRepository.AllAsNoTracking().Where(c => c.ApplicationUserId == userId).To<T>().ToList();
        }

        public T GetByOrderNumber<T>(string orderNumber)
        {
            return this.orderRepository.AllAsNoTracking().Where(c => c.OrderNumber == orderNumber).To<T>().FirstOrDefault();
        }
    }
}
