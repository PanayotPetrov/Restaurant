namespace Restaurant.Web.Infrastructure.Tests
{
    using System.ComponentModel.DataAnnotations;
    using Moq;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Xunit;

    public class ReviewRelatedValidationAttributesTests : IClassFixture<ServiceCollectionFixture>
    {
        private readonly ServiceCollectionFixture fixture;

        public ReviewRelatedValidationAttributesTests(ServiceCollectionFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void ReviewIdIdValidation_IsValid_ShouldReturnTrueIfExists()
        {
            var reviewId = 1;

            this.fixture.ReviewServiceMock.Setup(x => x.IsReviewIdValid(It.IsAny<int>())).Returns(true);

            var attribute = new ReviewIdValidationAttribute();
            var result = attribute.GetValidationResult(reviewId, this.fixture.ValidationContext);
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void ReviewIdValidation_IsValid_ShouldReturnFalse_IfReviewDoesNotExists()
        {
            var reviewId = 1;

            this.fixture.ReviewServiceMock.Setup(x => x.IsReviewIdValid(It.IsAny<int>())).Returns(false);

            var attribute = new ReviewIdValidationAttribute();
            var result = attribute.GetValidationResult(reviewId, this.fixture.ValidationContext);
            Assert.NotEqual(ValidationResult.Success, result);
        }
    }
}
