namespace Restaurant.Web.ViewModels.Menu
{
    using System.Collections.Generic;

    using Restaurant.Web.ViewModels.Meal;

    public class MenuViewModel
    {
        public IEnumerable<MealViewModel> Meals { get; set; }
    }
}
