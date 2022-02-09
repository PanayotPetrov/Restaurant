namespace Restaurant.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Web.ViewModels.Meal;
    using Restaurant.Web.ViewModels.Menu;

    public class MenuController : BaseController
    {
        private readonly IMealService mealService;

        public MenuController(IMealService mealService)
        {
            this.mealService = mealService;
        }

        [HttpGet("/Menu/")]
        public IActionResult All()
        {
            var menu = new MenuViewModel { Meals = this.mealService.GetAllMeals<MealViewModel>() };

            return this.View(menu);
        }

        // TO DO: Add one more view - (show by categories?)
    }
}
