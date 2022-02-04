namespace Restaurant.Web.ViewModels.Meal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MealListViewModel
    {
        public ICollection<MealViewModel> Meals { get; set; }
    }
}
