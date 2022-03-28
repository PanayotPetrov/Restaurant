namespace Restaurant.Web.ViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;

    public class AddToCartInputModel : IMapTo<CartItemModel>
    {
        public int MealId { get; set; }

        [Range(1, 15)]
        public int Quantity { get; set; }
    }
}
