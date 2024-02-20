namespace Restaurant.Web.ViewModels.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Restaurant.Common.Resources;
    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Restaurant.Web.ViewModels.Cart;

    public class ChangeCartItemQuantityInputModel : IMapTo<CartItemModel>, IValidatableObject
    {
        [MealIdValidation]
        public int MealId { get; set; }

        [Range(1, 100, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_RANGE_CART_ITEM_QUANTITY")]
        public int Quantity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var httpContextAccessor = validationContext.GetService<IHttpContextAccessor>();
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cartService = validationContext.GetService<ICartService>();
            var cart = cartService.GetCartByUserId<CartViewModel>(userId);
            var previousItemQuantity = cart.CartItems.FirstOrDefault(ci => ci.Meal.Id == this.MealId).Quantity;
            var cartItemTotalQuantity = cart.CartItems.Select(ci => ci.Quantity).Sum();

            if (this.Quantity < previousItemQuantity)
            {
                yield return ValidationResult.Success;
            }
            else if (cartItemTotalQuantity < 100)
            {
                yield return ValidationResult.Success;
            }
            else
            {
                yield return new ValidationResult("You cannot have more than 100 meals per order.");
            }
        }
    }
}
