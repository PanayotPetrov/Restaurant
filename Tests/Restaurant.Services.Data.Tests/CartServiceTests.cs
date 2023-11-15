namespace Restaurant.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Restaurant.Data.Repositories;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels.Cart;
    using Xunit;

    [Collection("Mapper assembly")]
    public class CartServiceTests
    {
        private List<Cart> carts;
        private Mock<IRestaurantDeletableEntityRepositoryDecorator<Cart>> cartRepository;
        private Mock<IRestaurantRepositoryDecorator<CartItem>> cartItemRepository;
        private Mock<IRestaurantDeletableEntityRepositoryDecorator<Meal>> mealRepository;
        private CartService service;

        public CartServiceTests()
        {
            this.carts = new List<Cart>();
            this.cartRepository = new Mock<IRestaurantDeletableEntityRepositoryDecorator<Cart>>();
            this.cartItemRepository = new Mock<IRestaurantRepositoryDecorator<CartItem>>();
            this.mealRepository = new Mock<IRestaurantDeletableEntityRepositoryDecorator<Meal>>();

            this.service = new CartService(this.cartRepository.Object, this.cartItemRepository.Object, this.mealRepository.Object);
        }

        [Fact]
        public void GetCartByUserId_ShouldReturnCorrectCart()
        {
            var userId = "Test user id 1";
            this.GenerateCarts(2);

            this.cartRepository.Setup(cr => cr.AllAsNoTracking()).Returns(this.carts.AsQueryable());

            var result = this.service.GetCartByUserId<CartViewModel>(userId);

            Assert.Equal("14,00", result.TotalPrice);
        }

        [Fact]
        public void GetCartId_ShouldReturnCorrectCart()
        {
            var userId = "Test user id 1";
            this.GenerateCarts(2);

            this.cartRepository.Setup(cr => cr.AllAsNoTracking()).Returns(this.carts.AsQueryable());

            var result = this.service.GetCartId(userId);

            Assert.Equal(1, result);
        }

        [Fact]
        public void GetCartSubTotal_ShouldReturnCorrectValue()
        {
            var userId = "Test user id 1";
            this.GenerateCarts(2);

            this.cartRepository.Setup(cr => cr.AllAsNoTracking()).Returns(this.carts.AsQueryable());

            var result = this.service.GetCartSubTotal(userId);

            Assert.Equal(11, result);
        }

        [Fact]
        public void GetCartById_ShouldReturnCorrectValue()
        {
            this.GenerateCarts(2);

            this.cartRepository.Setup(cr => cr.AllAsNoTracking()).Returns(this.carts.AsQueryable());

            var result = this.service.GetCartById(1);

            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetItemQuantityPerCartLeft_ShouldReturnCorrectValue()
        {
            this.GenerateCarts(1);

            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    Id = 1,
                    Quantity = 25,
                    Cart = this.carts.FirstOrDefault(),
                },
                new CartItem
                {
                    Id = 2,
                    Quantity = 20,
                    Cart = this.carts.FirstOrDefault(),
                },
                new CartItem
                {
                    Id = 3,
                    Quantity = 5,
                    Cart = this.carts.FirstOrDefault(),
                },
            };

            this.cartItemRepository.Setup(cr => cr.AllAsNoTracking()).Returns(cartItems.AsQueryable);

            var result = this.service.GetItemQuantityPerCartLeft("Test user id 1");

            Assert.Equal(50, result);
        }

        [Fact]
        public async Task RemoveFromCartAsync_ShouldRemoveCartItem()
        {
            this.GenerateCarts(1);

            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    Id = 1,
                    MealId = 1,
                    Cart = this.carts.FirstOrDefault(),
                },
                new CartItem
                {
                    Id = 2,
                    MealId = 2,
                    Cart = this.carts.FirstOrDefault(),
                },
            };

            var model = new CartItemModel
            {
                MealId = 1,
                Quantity = 1,
            };

            this.cartItemRepository.Setup(ci => ci.All()).Returns(cartItems.AsQueryable());
            this.cartItemRepository.Setup(ci => ci.Delete(It.IsAny<CartItem>()))
                .Callback((CartItem cartItem) => cartItems.Remove(cartItem));

            await this.service.RemoveFromCartAsync(model);
            Assert.Single(cartItems);
        }

        [Fact]
        public async Task CreateCartForUser_ShouldCreateCart()
        {
            this.cartRepository.Setup(c => c.AddAsync(It.IsAny<Cart>())).Callback((Cart cart) => this.carts.Add(cart));

            await this.service.CreateCartForUserAsync<CartViewModel>("Test user id 1");
            Assert.Single(this.carts);
        }

        [Fact]
        public async Task ClearCartAsync_ShouldDeleteAllCartItems()
        {
            this.GenerateCarts(1);

            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    Id = 1,
                    MealId = 1,
                    CartId = 1,
                },
                new CartItem
                {
                    Id = 2,
                    MealId = 2,
                    CartId = 1,
                },
            };

            this.cartItemRepository.Setup(ci => ci.All()).Returns(cartItems.AsQueryable());
            this.cartRepository.Setup(c => c.All()).Returns(this.carts.AsQueryable());

            this.cartItemRepository.Setup(ci => ci.Delete(It.IsAny<CartItem>()))
                .Callback((CartItem cartItem) => cartItems.Remove(cartItem));

            await this.service.ClearCartAsync(1);

            Assert.Empty(cartItems);
        }

        [Fact]
        public async Task ClearCartAsync_ShouldResetCartPrices()
        {
            this.GenerateCarts(1);

            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    Id = 1,
                    MealId = 1,
                    CartId = 1,
                },
                new CartItem
                {
                    Id = 2,
                    MealId = 2,
                    CartId = 1,
                },
            };

            this.cartItemRepository.Setup(ci => ci.All()).Returns(cartItems.AsQueryable());
            this.cartRepository.Setup(c => c.All()).Returns(this.carts.AsQueryable());

            this.cartItemRepository.Setup(ci => ci.Delete(It.IsAny<CartItem>()))
                .Callback((CartItem cartItem) => cartItems.Remove(cartItem));

            await this.service.ClearCartAsync(1);

            Assert.Equal(0, this.carts.FirstOrDefault().SubTotal);
            Assert.Equal(3M, this.carts.FirstOrDefault().TotalPrice);
        }

        [Fact]
        public async Task AddToCartAsync_ShouldCreateCartForUser_IfUserDoesNotHaveAcart()
        {
            var meals = new List<Meal>
            {
                new Meal { Id = 1, Price = 10 },
            };

            var model = new CartItemModel
            {
                MealId = 1,
            };

            this.cartRepository.Setup(c => c.All()).Returns(this.carts.AsQueryable());
            this.cartRepository.Setup(c => c.AddAsync(It.IsAny<Cart>())).Callback((Cart cart) => this.carts.Add(cart));

            this.mealRepository.Setup(c => c.AllAsNoTracking()).Returns(meals.AsQueryable());

            await this.service.AddToCartAsync("Test user id 1", model);

            Assert.Single(this.carts);
        }

        [Fact]
        public async Task AddToCartAsync_ShouldAddItemToCart()
        {
            this.GenerateCarts(1);
            var meals = new List<Meal>
            {
                new Meal { Id = 1, Price = 10 },
            };

            var model = new CartItemModel
            {
                MealId = 1,
                Quantity = 10,
            };

            this.cartRepository.Setup(c => c.All()).Returns(this.carts.AsQueryable());

            this.mealRepository.Setup(c => c.AllAsNoTracking()).Returns(meals.AsQueryable());

            await this.service.AddToCartAsync("Test user id 1", model);

            Assert.Single(this.carts.FirstOrDefault().CartItems);
        }

        [Fact]
        public async Task AddToCartAsync_ShouldAddChangeItemQuantity_IfAlreadyAddedToCart()
        {
            this.GenerateCarts(1);
            this.carts.FirstOrDefault().CartItems.Add(new CartItem { MealId = 1, Quantity = 5, });
            var meals = new List<Meal>
            {
                new Meal { Id = 1, Price = 10 },
            };

            var model = new CartItemModel
            {
                MealId = 1,
                Quantity = 10,
            };

            this.cartRepository.Setup(c => c.All()).Returns(this.carts.AsQueryable());

            this.mealRepository.Setup(c => c.AllAsNoTracking()).Returns(meals.AsQueryable());

            await this.service.AddToCartAsync("Test user id 1", model);

            Assert.Equal(15, this.carts.FirstOrDefault().CartItems.Sum(x => x.Quantity));
        }

        [Fact]
        public async Task AddToCartAsync_ShouldUpdateTotalPrice_AndCartItemTotalPrice()
        {
            this.GenerateCarts(1);
            this.carts.FirstOrDefault().CartItems.Add(new CartItem { MealId = 1, Quantity = 5, ItemTotalPrice = 50 });
            var meals = new List<Meal>
            {
                new Meal { Id = 1, Price = 10 },
            };

            var model = new CartItemModel
            {
                MealId = 1,
                Quantity = 10,
            };

            this.cartRepository.Setup(c => c.All()).Returns(this.carts.AsQueryable());

            this.mealRepository.Setup(c => c.AllAsNoTracking()).Returns(meals.AsQueryable());

            await this.service.AddToCartAsync("Test user id 1", model);

            var cartTotalPrice = this.carts.FirstOrDefault().TotalPrice;
            var cartItemTotalPrice = this.carts.FirstOrDefault().CartItems.FirstOrDefault().ItemTotalPrice;

            Assert.Equal(150, cartItemTotalPrice);
            Assert.Equal(114, cartTotalPrice);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(5)]

        public async Task ChangeItemQuantityAsync_ShouldUpdateTotalPrice_ItemQuantity_AndCartItemTotalPrice(int newMealQuantity)
        {
            var testMeal = new Meal { Id = 1, Price = 10 };

            var cartItems = new List<CartItem> { new CartItem { MealId = 1, Quantity = 4, CartId = 1, Meal = testMeal, } };

            var cart = new Cart
            {
                Id = 1,
                ApplicationUserId = "Test user id 1",
                SubTotal = 40M,
                TotalPrice = 43.9M,
                ShippingPrice = 3.9M,
                CartItems = cartItems,
            };
            this.carts.Add(cart);

            var meals = new List<Meal> { testMeal };

            var model = new CartItemModel
            {
                CartId = 1,
                MealId = 1,
                Quantity = newMealQuantity,
            };

            this.cartRepository.Setup(c => c.All()).Returns(this.carts.AsQueryable());
            this.cartItemRepository.Setup(c => c.All()).Returns(cartItems.AsQueryable());

            this.mealRepository.Setup(c => c.AllAsNoTracking()).Returns(meals.AsQueryable());

            await this.service.ChangeItemQuantityAsync<CartItemViewModel>(model);

            var cartTotalPrice = this.carts.FirstOrDefault().TotalPrice;
            var cartItemTotalPrice = this.carts.FirstOrDefault().CartItems.FirstOrDefault().ItemTotalPrice;
            var cartItemQuantity = this.carts.FirstOrDefault().CartItems.FirstOrDefault().Quantity;

            if (newMealQuantity == 3)
            {
                Assert.Equal(30M, cartItemTotalPrice);
                Assert.Equal(43.9M, cartTotalPrice);
                Assert.Equal(3, cartItemQuantity);
            }
            else if (newMealQuantity == 5)
            {
                Assert.Equal(50, cartItemTotalPrice);
                Assert.Equal(53.9m, cartTotalPrice);
                Assert.Equal(5, cartItemQuantity);
            }
        }

        private void GenerateCarts(int numberOfCarts)
        {
            for (int i = 1; i <= numberOfCarts; i++)
            {
                this.carts.Add(new Cart
                {
                    Id = i,
                    ApplicationUserId = $"Test user id {i}",
                    SubTotal = 10 + i,
                    TotalPrice = 13 + i,
                    ShippingPrice = 3,
                });
            }
        }
    }
}
