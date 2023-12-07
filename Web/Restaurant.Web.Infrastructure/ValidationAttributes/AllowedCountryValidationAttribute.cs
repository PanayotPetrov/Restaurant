namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    public class AllowedCountryValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // TO DO: GET FROM CONFIG
            var alowedCountry = "Bulgaria";

            if (value is string stringValue)
            {
                if (stringValue.ToLower() == alowedCountry.ToLower())
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult($"We currently deliver only in {alowedCountry}");
        }
    }
}
