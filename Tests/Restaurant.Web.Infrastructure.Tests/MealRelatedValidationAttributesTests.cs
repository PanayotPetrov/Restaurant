﻿namespace Restaurant.Web.Infrastructure.Tests
{
    using System.ComponentModel.DataAnnotations;

    using Moq;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Xunit;

    [Collection("Services")]
    public class MealRelatedValidationAttributesTests
    {
        private readonly Services fixture;

        public MealRelatedValidationAttributesTests(Services fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void MealIdValidation_IsValid_ShouldReturnTrueIfExists()
        {
            var mealId = 1;

            this.fixture.MealServiceMock.Setup(x => x.IsMealIdValid(It.IsAny<int>())).Returns(true);

            var attribute = new MealIdValidationAttribute();
            var result = attribute.GetValidationResult(mealId, this.fixture.ValidationContext);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void MealIdValidation_IsValid_ShouldReturnFalseIfMealIdDoesNotExist()
        {
            var mealId = 1;

            this.fixture.MealServiceMock.Setup(x => x.IsMealIdValid(It.IsAny<int>())).Returns(false);

            var attribute = new MealIdValidationAttribute();
            var result = attribute.GetValidationResult(mealId, this.fixture.ValidationContext);

            Assert.NotEqual(ValidationResult.Success, result);
        }
    }
}
