namespace Restaurant.Web.ViewModels.Menu
{
    using System.Collections.Generic;
    using Restaurant.Web.ViewModels.Cart;
    using Restaurant.Web.ViewModels.Category;

    public class MenuViewModel
    {
        public IEnumerable<CartItemViewModel> CartItems { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
