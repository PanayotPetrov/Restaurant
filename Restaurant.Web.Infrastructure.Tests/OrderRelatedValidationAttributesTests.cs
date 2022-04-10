namespace Restaurant.Web.Infrastructure.Tests
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Moq;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Xunit;

    public class OrderRelatedValidationAttributesTests : IClassFixture<ServiceCollectionFixture>
    {
        private ServiceCollectionFixture fixture;

        public OrderRelatedValidationAttributesTests(ServiceCollectionFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void OrderNumberValidation_IsValid_ShouldReturnTrueIfExists()
        {
            var orderNumber = "Test order number";

            var orderNumbers = new List<string>
            {
                "Test order number",
                "Test order number2",
                "Test order number3",
            };

            this.fixture.OrderServiceMock.Setup(x => x.GetAllOrderNumbersByUserId(It.IsAny<string>())).Returns(orderNumbers);

            var attribute = new OrderNumberValidationAttribute();
            var result = attribute.GetValidationResult(orderNumber, this.fixture.ValidationContext);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void OrderNumberValidation_IsValid_ShouldReturnFalse_IfOrderNumberDoesNotExit()
        {
            var orderNumber = "Test order number";

            var orderNumbers = new List<string>
            {
                "Test order number1",
                "Test order number2",
                "Test order number3",
            };

            this.fixture.OrderServiceMock.Setup(x => x.GetAllOrderNumbersByUserId(It.IsAny<string>())).Returns(orderNumbers);

            var attribute = new OrderNumberValidationAttribute();
            var result = attribute.GetValidationResult(orderNumber, this.fixture.ValidationContext);

            Assert.NotEqual(ValidationResult.Success, result);
        }
    }
}
