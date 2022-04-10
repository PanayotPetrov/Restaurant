namespace Restaurant.Web.Infrastructure.Tests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Moq;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Xunit;

    [Collection("Services")]
    public class ReservationRelatedValidationAttributesTests
    {
        private readonly Services fixture;

        public ReservationRelatedValidationAttributesTests(Services fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void ReservationIdValidation_ShouldReturnTrue_IfIdIsValid()
        {
            var reservationId = "Test id";

            this.fixture.ReservationServiceMock.Setup(r => r.IsValid(It.IsAny<string>())).Returns(true);

            var attribute = new ReservationIdValidationAttribute();
            var result = attribute.GetValidationResult(reservationId, this.fixture.ValidationContext);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void ReservationIdValidation_ShouldReturnFalse_IfIdIsInvalid()
        {
            var reservationId = "Test id";

            this.fixture.ReservationServiceMock.Setup(r => r.IsValid(It.IsAny<string>())).Returns(false);

            var attribute = new ReservationIdValidationAttribute();
            var result = attribute.GetValidationResult(reservationId, this.fixture.ValidationContext);

            Assert.NotEqual(ValidationResult.Success, result);
        }

        [Fact]
        public void CurrentDateValidation_ShouldReturnFalse_IfValueIsInThePast()
        {
            var date = DateTime.Now.Subtract(TimeSpan.FromDays(1));

            var attribute = new CurrentDateValidationAttribute();
            var result = attribute.GetValidationResult(date, this.fixture.ValidationContext);

            Assert.NotEqual(ValidationResult.Success, result);
        }

        [Fact]
        public void CurrentDateValidation_ShouldReturnFalse_IfValueIsTooFarInTheFuture()
        {
            var date = DateTime.Now.AddMonths(2);

            var attribute = new CurrentDateValidationAttribute();
            var result = attribute.GetValidationResult(date, this.fixture.ValidationContext);

            Assert.NotEqual(ValidationResult.Success, result);
        }

        [Fact]
        public void CurrentDateValidation_ShouldReturnTrue_IfValueIsInRange()
        {
            var date = DateTime.Now.AddDays(15);

            var attribute = new CurrentDateValidationAttribute();
            var result = attribute.GetValidationResult(date, this.fixture.ValidationContext);

            Assert.Equal(ValidationResult.Success, result);
        }
    }
}
