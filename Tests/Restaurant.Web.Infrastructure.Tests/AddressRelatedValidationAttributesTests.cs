namespace Restaurant.Web.Infrastructure.Tests
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Moq;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Xunit;

    [Collection("Services")]
    public class AddressRelatedValidationAttributesTests
    {
        private Services fixture;

        public AddressRelatedValidationAttributesTests(Services fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void AddressNameValidation_IsValid_ShouldReturnTrue_IfAddressIsValid()
        {
            var addressNames = new List<string> { "first", "second" };

            this.fixture.AddressServiceMock.Setup(x => x.GetAddressNamesByUserId(It.IsAny<string>())).Returns(addressNames);

            var attribute = new AddressNameValidationAttribute();

            var result = attribute.GetValidationResult("first", this.fixture.ValidationContext);
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void AddressNameValidation_IsValid_ShouldReturnFalse_IfAddressNameIsInvalid()
        {
            // TO DO: ADD MOCK FOR ISharedViewLocalizer
            var addressNames = new List<string> { "first", "second" };

            this.fixture.AddressServiceMock.Setup(x => x.GetAddressNamesByUserId(It.IsAny<string>())).Returns(addressNames);

            var attribute = new AddressNameValidationAttribute();

            var result = attribute.GetValidationResult("third", this.fixture.ValidationContext);
            Assert.NotEqual(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData("Test")]
        [InlineData(null)]
        [InlineData("")]
        public void AllowedCityValidation_IsValid_ShouldReturnFalse_IfCityIsInvalid(string city)
        {
            var attribute = new AllowedCityValidationAttribute();

            var result = attribute.GetValidationResult(city, this.fixture.ValidationContext);
            Assert.NotEqual(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData("Sofia")]
        [InlineData("SOFIA")]

        public void AllowedCityValidation_IsValid_ShouldReturnTrue_IfCityIsValid(string city)
        {
            var attribute = new AllowedCityValidationAttribute();

            var result = attribute.GetValidationResult(city, this.fixture.ValidationContext);
            Assert.Equal(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData("Test")]
        [InlineData(null)]
        [InlineData("")]

        public void AllowedCountryValidation_IsValid_ShouldReturnFalse_IfCountryIsInvalid(string country)
        {
            var attribute = new AllowedCountryValidationAttribute();

            var result = attribute.GetValidationResult(country, this.fixture.ValidationContext);
            Assert.NotEqual(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData("Bulgaria")]
        [InlineData("BULGARIA")]

        public void AllowedCountryValidation_IsValid_ShouldReturnTrue_IfCountryIsValid(string country)
        {
            var attribute = new AllowedCountryValidationAttribute();

            var result = attribute.GetValidationResult(country, this.fixture.ValidationContext);
            Assert.Equal(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData("Test")]
        [InlineData(null)]
        [InlineData("")]

        public void AllowedDistrictsValidation_IsValid_ShouldReturnFalse_IfDistrictIsInvalid(string district)
        {
            var districts = new List<string>
            {
                "Lagera",
                "Centur",
                "Hipodruma",
            };

            this.fixture.AddressServiceMock.Setup(x => x.GetAllowedDistricts()).Returns(districts);

            var attribute = new AllowedDistrictsValidationAttribute();

            var result = attribute.GetValidationResult(district, this.fixture.ValidationContext);
            Assert.NotEqual(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData("Lagera")]
        [InlineData("Centur")]

        public void AllowedDistrictsValidation_IsValid_ShouldReturnTrue_IfDistrictIsValid(string district)
        {
            var districts = new List<string>
            {
                "Lagera",
                "Centur",
                "Hipodruma",
                "Mladost",
            };

            this.fixture.AddressServiceMock.Setup(x => x.GetAllowedDistricts()).Returns(districts);

            var attribute = new AllowedDistrictsValidationAttribute();

            var result = attribute.GetValidationResult(district, this.fixture.ValidationContext);
            Assert.Equal(ValidationResult.Success, result);
        }
    }
}
