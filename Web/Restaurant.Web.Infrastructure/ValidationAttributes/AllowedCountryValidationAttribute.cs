namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class AllowedCountryValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // TO DO: GET FROM CONFIG
            List<string> allowedCountries = ["Bulgaria", "България"];

            if (value is string stringValue)
            {
                if (allowedCountries.Any(c => c.Equals(stringValue, System.StringComparison.CurrentCultureIgnoreCase)))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
