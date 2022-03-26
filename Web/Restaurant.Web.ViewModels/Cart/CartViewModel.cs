namespace Restaurant.Web.ViewModels.Cart
{
    using System.Collections.Generic;

    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class CartViewModel : IMapFrom<Cart>
    {
        public IEnumerable<CartItemViewModel> CartItems { get; set; }
    }
}
