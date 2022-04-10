namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    public class AllowedCityValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var allowedCity = "Sofia";

            if (value is string stringValue)
            {
                if (stringValue.ToLower() == allowedCity.ToLower())
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult($"We currently deliver only in {allowedCity}");
        }
    }
}
