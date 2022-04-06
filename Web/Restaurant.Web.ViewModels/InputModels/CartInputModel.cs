namespace Restaurant.Web.ViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.Infrastructure.ValidationAttributes;

    public class CartInputModel : IMapTo<CartItemModel>
    {
        [MealIdValidation]
        public int MealId { get; set; }

        [Range(1, 100)]
        [CartItemQuantityValidation]
        public int Quantity { get; set; }
    }
}
