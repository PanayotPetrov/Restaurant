namespace Restaurant.Web.Infrastructure.Tests
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Moq;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Restaurant.Web.ViewModels.Cart;
    using Restaurant.Web.ViewModels.InputModels;
    using Restaurant.Web.ViewModels.Meal;
    using Xunit;

    public class CartRelatedValidationAttributesTests : IClassFixture<ServiceCollectionFixture>
    {
        private ServiceCollectionFixture fixture;

        public CartRelatedValidationAttributesTests(ServiceCollectionFixture fixture)
        {
            this.fixture = fixture;
        }

        [Theory]
        [InlineData(1)]

        public void CartIdValidation_IsValid_ShouldReturnTrue_IfCartIdIsCorrect(int cartIdInput)
        {
            var cartId = 1;

            this.fixture.CartServiceMock.Setup(x => x.GetCartId(It.IsAny<string>())).Returns(cartId);

            var attribute = new CartIdValidationAttribute();

            var result = attribute.GetValidationResult(cartIdInput, this.fixture.ValidationContext);
            Assert.Equal(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(null)]

        public void CartIdValidation_IsValid_ShouldReturnFalse_IfCartIdIsInvalid(int? cartIdInput)
        {
            var cartId = 1;

            this.fixture.CartServiceMock.Setup(x => x.GetCartId(It.IsAny<string>())).Returns(cartId);

            var attribute = new CartIdValidationAttribute();

            var result = attribute.GetValidationResult(cartIdInput, this.fixture.ValidationContext);
            Assert.NotEqual(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(100)]

        public void CartItemQuantityValidation_IsValid_ShouldReturnTrue_CartHasEnoughItemSpaceLeft(int quantityToAdd)
        {
            var cartItemSpaceLeft = 100;

            this.fixture.CartServiceMock.Setup(x => x.GetItemQuantityPerCartLeft(It.IsAny<string>())).Returns(cartItemSpaceLeft);

            var attribute = new CartItemQuantityValidationAttribute();

            var result = attribute.GetValidationResult(quantityToAdd, this.fixture.ValidationContext);
            Assert.Equal(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData(101, 100)]
        [InlineData(15, 10)]

        public void CartItemQuantityValidation_IsValid_ShouldReturnFalse_CartCannotFitThatManyItems(int quantityToAdd, int cartItemSpaceLeft)
        {
            this.fixture.CartServiceMock.Setup(x => x.GetItemQuantityPerCartLeft(It.IsAny<string>())).Returns(cartItemSpaceLeft);

            var attribute = new CartItemQuantityValidationAttribute();

            var result = attribute.GetValidationResult(quantityToAdd, this.fixture.ValidationContext);
            Assert.NotEqual(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData(1, 2)]

        public void ChangeCartItemQuantityInputModel_ShouldReturnTrue_WhenTryingToReduceQuantity(int newItemQuantity, int previousQuantity)
        {
            var model = new ChangeCartItemQuantityInputModel();
            model.MealId = 1;
            model.Quantity = newItemQuantity;

            var cartViewModel = new CartViewModel
            {
                CartItems = new List<CartItemViewModel>
                {
                    new CartItemViewModel
                    {
                        Quantity = previousQuantity,
                        Meal = new MealViewModel { Id = 1 },
                    },
                },
            };

            this.fixture.CartServiceMock.Setup(x => x.GetCartByUserId<CartViewModel>(It.IsAny<string>())).Returns(cartViewModel);

            var results = model.Validate(this.fixture.ValidationContext);

            foreach (var result in results)
            {
                Assert.Equal(ValidationResult.Success, result);
            }
        }

        [Theory]
        [InlineData(100, 99)]

        public void ChangeCartItemQuantityInputModel_ShouldReturnFalse_IfCartItemCapacityIsFull(int newItemQuantity, int previousQuantity)
        {
            var model = new ChangeCartItemQuantityInputModel();
            model.MealId = 1;
            model.Quantity = newItemQuantity;

            var cartViewModel = new CartViewModel
            {
                CartItems = new List<CartItemViewModel>
                {
                    new CartItemViewModel
                    {
                        Quantity = previousQuantity,
                        Meal = new MealViewModel { Id = 1 },
                    },
                    new CartItemViewModel
                    {
                        Quantity = 1,
                        Meal = new MealViewModel { Id = 2 },
                    },
                },
            };

            this.fixture.CartServiceMock.Setup(x => x.GetCartByUserId<CartViewModel>(It.IsAny<string>())).Returns(cartViewModel);

            var results = model.Validate(this.fixture.ValidationContext);

            foreach (var result in results)
            {
                Assert.NotEqual(ValidationResult.Success, result);
            }
        }

        [Theory]
        [InlineData(99, 98)]

        public void ChangeCartItemQuantityInputModel_ShouldReturnTrue_IfItemQuantityCanBeIncreased(int newItemQuantity, int previousQuantity)
        {
            var model = new ChangeCartItemQuantityInputModel();
            model.MealId = 1;
            model.Quantity = newItemQuantity;

            var cartViewModel = new CartViewModel
            {
                CartItems = new List<CartItemViewModel>
                {
                    new CartItemViewModel
                    {
                        Quantity = previousQuantity,
                        Meal = new MealViewModel { Id = 1 },
                    },
                    new CartItemViewModel
                    {
                        Quantity = 1,
                        Meal = new MealViewModel { Id = 2 },
                    },
                },
            };

            this.fixture.CartServiceMock.Setup(x => x.GetCartByUserId<CartViewModel>(It.IsAny<string>())).Returns(cartViewModel);

            var results = model.Validate(this.fixture.ValidationContext);

            foreach (var result in results)
            {
                Assert.Equal(ValidationResult.Success, result);
            }
        }
    }
}
