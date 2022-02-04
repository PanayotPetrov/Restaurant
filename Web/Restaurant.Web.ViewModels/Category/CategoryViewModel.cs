namespace Restaurant.Web.ViewModels.Category
{
    using System.Collections.Generic;

    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Web.ViewModels.Meal;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public string Name { get; set; }

        public IEnumerable<MealViewModel> Meals { get; set; }
    }
}
