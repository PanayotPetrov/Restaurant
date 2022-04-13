namespace Restaurant.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels.Order;
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
        public void GetAllOrderNumbersByUserId_ShouldReturnCorrectValue()
        {
            this.GenerateOrders(3);

            this.repository.Setup(x => x.AllAsNoTracking()).Returns(this.orders.AsQueryable());

            var result = this.service.GetAllOrderNumbersByUserId("Test user id 1");

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetByUserIdAndOrderNumber_ShouldReturnCorrectValue()
        {
            this.GenerateOrders(5);

            this.repository.Setup(x => x.AllAsNoTracking()).Returns(this.orders.AsQueryable());

            var result = this.service.GetByUserIdAndOrderNumber<OrderViewModel>("Test user id 1", "Test orderNumber 3");

            Assert.Equal("Test orderNumber 3", result.OrderNumber);
        }

        [Fact]
        public void GetAllOrderNumbers_ShouldReturnCorrectValue()
        {
            this.GenerateOrders(3);

            this.repository.Setup(x => x.AllAsNoTrackingWithDeleted()).Returns(this.orders.AsQueryable());

            var result = this.service.GetAllOrderNumbers();

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void GetAllWithPagination_ShouldReturnCorrectValue()
        {
            this.GenerateOrders(5);

            this.repository.Setup(x => x.AllAsNoTrackingWithDeleted()).Returns(this.orders.AsQueryable());

            var result = this.service.GetAllWithPagination<AdminOrderViewModel>(3, 1);
            var result2 = this.service.GetAllWithPagination<AdminOrderViewModel>(3, 2);

            Assert.Equal(3, result.Count());
            Assert.Equal(2, result2.Count());
        }

        [Fact]
        public void GetCountWithDeleted_ShouldReturnCorrectValue()
        {
            this.GenerateOrders(3);

            this.repository.Setup(x => x.AllAsNoTrackingWithDeleted()).Returns(this.orders.AsQueryable());

            var result = this.service.GetCountWithDeleted();

            Assert.Equal(3, result);
        }

        [Fact]
        public void GetByOrderNumberWithDeleted_ShouldReturnCorrectOrder()
        {
            this.GenerateOrders(3);

            this.repository.Setup(x => x.AllAsNoTrackingWithDeleted()).Returns(this.orders.AsQueryable());

            var result = this.service.GetByOrderNumberWithDeleted<AdminOrderViewModel>("Test orderNumber 3");

            Assert.Equal("Test orderNumber 3", result.OrderNumber);
        }

        [Fact]
        public async Task RestoreAsync_ShouldReturnFalseIfOrderIsNotDeleted()
        {
            this.orders.Add(new Order
            {
                OrderNumber = "Test order number",
                IsDeleted = false,
            });

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.orders.AsQueryable());
            Assert.False(await this.service.RestoreAsync("Test order number"));
        }

        [Fact]
        public async Task RestoreAsync_ShouldReturnTrueIfOrderIsRestored()
        {
            this.orders.Add(new Order
            {
                OrderNumber = "Test order number",
                IsDeleted = true,
            });

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.orders.AsQueryable());
            Assert.True(await this.service.RestoreAsync("Test order number"));
        }

        [Fact]
        public async Task RestoreAsync_ShouldRestoreOrder()
        {
            this.orders.Add(new Order
            {
                OrderNumber = "Test order number",
                IsDeleted = true,
            });

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.orders.AsQueryable());
            await this.service.RestoreAsync("Test order number");
            Assert.False(this.orders.FirstOrDefault().IsDeleted);
        }

        [Fact]
        public async Task DeleteByOrderNumberAsync_ShouldReturnTrueIfSuccessfullyDeleted()
        {
            this.orders.Add(new Order
            {
                OrderNumber = "Test order number",
                IsDeleted = false,
            });

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.orders.AsQueryable());
            Assert.True(await this.service.DeleteByOrderNumberAsync("Test order number"));
        }

        [Fact]
        public async Task DeleteByOrderNumberAsync__ShouldDeleteOrder()
        {
            this.orders.Add(new Order
            {
                OrderNumber = "Test order number",
                IsDeleted = false,
            });

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.orders.AsQueryable());
            this.repository.Setup(r => r.Delete(It.IsAny<Order>())).Callback((Order order) => this.orders.Remove(order));
            await this.service.DeleteByOrderNumberAsync("Test order number");
            Assert.Empty(this.orders);
        }

        [Fact]
        public async Task DeleteByOrderNumberAsync__ShouldReturnFalseIfOrderAlreadyDeleted()
        {
            this.orders.Add(new Order
            {
                OrderNumber = "Test order number",
                IsDeleted = true,
            });

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.orders.AsQueryable());
            Assert.False(await this.service.DeleteByOrderNumberAsync("Test order number"));
        }

        [Fact]
        public async Task CompleteAsync_ShouldReturnFalseIfOrderIsAlreadyComplete()
        {
            this.orders.Add(new Order
            {
                OrderNumber = "Test order number",
                IsComplete = true,
            });

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.orders.AsQueryable());
            Assert.False(await this.service.CompleteAsync("Test order number"));
        }

        [Fact]
        public async Task CompleteAsync_ShouldReturnTrueIfOrderHasBeenComplete()
        {
            this.orders.Add(new Order
            {
                OrderNumber = "Test order number",
                IsComplete = false,
            });

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.orders.AsQueryable());
            Assert.True(await this.service.CompleteAsync("Test order number"));
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateOrder()
        {
            var model = new AddOrderModel
            {
                CartId = 1,
                AddressId = 1,
                ApplicationUserId = "Test user id",
            };

            var cart = new Cart
            {
                Id = 1,
                CartItems = new List<CartItem>
                {
                    new CartItem
                    {
                        Id = 1,
                        Quantity = 1,
                    },
                    new CartItem
                    {
                        Id = 2,
                        Quantity = 2,
                    },
                },
                TotalPrice = 30,
            };

            this.repository.Setup(r => r.AddAsync(It.IsAny<Order>()))
                .Callback((Order order) => this.orders.Add(order));

            this.cartService.Setup(cs => cs.GetCartById(It.IsAny<int>())).Returns(cart);

            await this.service.CreateAsync(model);
            Assert.Single(this.orders);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddCorrectNumberOfMeals()
        {
            var model = new AddOrderModel
            {
                CartId = 1,
                AddressId = 1,
                ApplicationUserId = "Test user id",
            };

            var cart = new Cart
            {
                Id = 1,
                CartItems = new List<CartItem>
                {
                    new CartItem
                    {
                        Id = 1,
                        Quantity = 1,
                    },
                    new CartItem
                    {
                        Id = 2,
                        Quantity = 2,
                    },
                },
                TotalPrice = 30,
            };

            this.repository.Setup(r => r.AddAsync(It.IsAny<Order>()))
                .Callback((Order order) => this.orders.Add(order));

            this.cartService.Setup(cs => cs.GetCartById(It.IsAny<int>())).Returns(cart);
            await this.service.CreateAsync(model);
            Assert.Equal(2, this.orders.FirstOrDefault().Meals.Count);
        }

        private void GenerateOrders(int numberOfOrders)
        {
            for (int i = 1; i <= numberOfOrders; i++)
            {
                this.orders.Add(new Order
                {
                    OrderNumber = $"Test orderNumber {i}",
                    ApplicationUserId = i % 2 != 0 ? $"Test user id 1" : $"Test user id 2",
                    ApplicationUser = new ApplicationUser { Cart = new Cart() },
                });
            }
        }
    }
}
