namespace Restaurant.Web.ViewModels.Meal
{
    using System.Collections.Generic;

    public class AdminMealListViewModel : PagingViewModel
    {
        public IEnumerable<AdminMealViewModel> Meals { get; set; }
    }
}
