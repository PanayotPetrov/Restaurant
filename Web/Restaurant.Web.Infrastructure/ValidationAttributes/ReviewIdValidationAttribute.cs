namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.Extensions.DependencyInjection;
    using Restaurant.Services.Data;

    public class ReviewIdValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int reviewId)
            {
                var reviewService = validationContext.GetService<IReviewService>();
                var result = reviewService.IsReviewIdValid(reviewId);
                if (result)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("Invalid review id provided.");
        }
    }
}
