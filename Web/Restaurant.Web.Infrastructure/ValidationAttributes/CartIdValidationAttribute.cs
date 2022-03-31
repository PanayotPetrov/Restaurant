namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Restaurant.Services.Data;

    public class CartIdValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int intValue)
            {
                var httpContextAccessor = validationContext.GetService<IHttpContextAccessor>();
                var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var cartService = validationContext.GetService<ICartService>();
                var cartId = cartService.GetCartId(userId);

                if (cartId == intValue)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("Invalid cart id provided");
        }
    }
}
