namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class AllowedCityValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // TO DO: Get values from config.
            List<string> allowedCities = ["Sofia", "София"];

            if (value is string stringValue)
            {
                if (allowedCities.Any(c => c.Equals(stringValue, System.StringComparison.CurrentCultureIgnoreCase)))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
