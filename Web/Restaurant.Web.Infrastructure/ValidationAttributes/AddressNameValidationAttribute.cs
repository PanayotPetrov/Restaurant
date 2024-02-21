namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Restaurant.Services.Data;
    using Restaurant.Web.HelperClasses;

    public class AddressNameValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var localizer = validationContext.GetService<ISharedViewLocalizer>();

            if (value is string stringValue)
            {
                var httpContextAccessor = validationContext.GetService<IHttpContextAccessor>();
                var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var addressService = validationContext.GetService<IAddressService>();
                var addressNames = addressService.GetAddressNamesByUserId(userId);

                if (addressNames.Contains(stringValue))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(localizer["VALIDATION_INVALID_ADDRESS_NAME"].Value);
            }

            return new ValidationResult(localizer["VALIDATION_NO_ADDRESS_SELECTED"].Value);
        }
    }
}
