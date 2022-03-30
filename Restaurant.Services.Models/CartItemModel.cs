namespace Restaurant.Services.Models
{
    using AutoMapper;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    public class CartItemModel : IMapTo<CartItem>
    {
        public int MealId { get; set; }

        public int Quantity { get; set; }
    }
}
