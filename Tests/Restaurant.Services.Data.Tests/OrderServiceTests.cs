namespace Restaurant.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Xunit;

    [Collection("Mapper assembly")]
    public class OrderServiceTests
    {
        private List<Order> orders;
        private Mock<IDeletableEntityRepository<Order>> repository;
        private Mock<ICartService> cartService;
        private OrderService service;

        public OrderServiceTests()
        {
            this.orders = new List<Order>();
            this.repository = new Mock<IDeletableEntityRepository<Order>>();
            this.cartService = new Mock<ICartService>();
            this.service = new OrderService(this.repository.Object, this.cartService.Object);
        }

        [Fact]
        public void GetByUserIdAndOrderNumber_ShouldReturnCorrectValue()
        {
            this.orders.Add(new Order
            {
                OrderNumber = "Test orderNumber",
                ApplicationUserId = "Test user id",
            });

            this.orders.Add(new Order
            {
                OrderNumber = "Test orderNumber",
                ApplicationUserId = "Test user id2",
            });
            var result = this.service.GetAllOrderNumbersByUserId("Test user id");

            this.repository.Setup(x => x.AllAsNoTracking()).Returns(this.orders.AsQueryable());

            Assert.Single(result);
        }
    }
}
