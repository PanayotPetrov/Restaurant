namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Restaurant.Services.Data;

    public class CartItemQuantityValidationAttribute : ValidationAttribute
    {
        private readonly List<string> actionsToValidate;

        public CartItemQuantityValidationAttribute(params string[] args)
        {
            this.actionsToValidate = args.ToList();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var httpContextAccessor = validationContext.GetService<IHttpContextAccessor>();
            var action = httpContextAccessor.HttpContext.Request.Path.ToString().Split('/').Last();

            if (!this.actionsToValidate.Contains(action))
            {
                return ValidationResult.Success;
            }

            var intValue = (int)value;
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
