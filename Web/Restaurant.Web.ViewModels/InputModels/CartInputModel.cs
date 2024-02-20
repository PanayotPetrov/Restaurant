namespace Restaurant.Web.ViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Common.Resources;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.Infrastructure.ValidationAttributes;

    public class CartInputModel : IMapTo<CartItemModel>
    {
        [MealIdValidation]
        public int MealId { get; set; }

        [Range(1, 100, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_RANGE")]
        [CartItemQuantityValidation]
        public int Quantity { get; set; }
    }
}
