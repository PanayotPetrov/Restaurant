namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Restaurant.Services.Data;

    public class CartItemQuantityValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var intValue = (int)value;
            var httpContextAccessor = validationContext.GetService<IHttpContextAccessor>();
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cartService = validationContext.GetService<ICartService>();
            var cartItemQuantityLeft = cartService.GetItemQuantityPerCartLeft(userId);

            if (cartItemQuantityLeft >= intValue)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"You cannot have more than 100 items per order. You can add a mixumum of {cartItemQuantityLeft} additional items.");
        }
    }
}
