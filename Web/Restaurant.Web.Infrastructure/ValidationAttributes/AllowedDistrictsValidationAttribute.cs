namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Microsoft.Extensions.DependencyInjection;
    using Restaurant.Services.Data;

    public class AllowedDistrictsValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string stringValue)
            {
                var addressService = validationContext.GetService<IAddressService>();
                var allowedDistricts = addressService.GetAllowedDistricts();
                if (allowedDistricts.Contains(stringValue))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
