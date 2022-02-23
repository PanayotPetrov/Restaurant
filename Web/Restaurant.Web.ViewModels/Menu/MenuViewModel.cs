namespace Restaurant.Web.ViewModels.Menu
{
    using System.Collections.Generic;

    using Restaurant.Web.ViewModels.Category;
    using Restaurant.Web.ViewModels.Meal;

    public class MenuViewModel
    {
        public IEnumerable<MealViewModel> Meals { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
