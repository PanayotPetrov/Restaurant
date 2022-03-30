namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.Extensions.DependencyInjection;
    using Restaurant.Services.Data;

    public class MealIdValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int mealId)
            {
                var mealService = validationContext.GetService<IMealService>();
                var result = mealService.IsMealIdValid(mealId);
                if (result)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("Invalid meal id provided.");
        }
    }
}
