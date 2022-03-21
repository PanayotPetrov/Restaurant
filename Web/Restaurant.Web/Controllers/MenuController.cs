namespace Restaurant.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Web.ViewModels.Category;
    using Restaurant.Web.ViewModels.Meal;
    using Restaurant.Web.ViewModels.Menu;

    public class MenuController : BaseController
    {
        private readonly IMealService mealService;
        private readonly ICategoryService categoryService;

        public MenuController(IMealService mealService, ICategoryService categoryService)
        {
            this.mealService = mealService;
            this.categoryService = categoryService;
        }

        [HttpGet("/Menu/")]
        public IActionResult AllMeals()
        {
            var menu = new MenuViewModel { Meals = this.mealService.GetAllMeals<MealViewModel>(), Categories = this.categoryService.GetAll<CategoryViewModel>() };

            return this.View(menu);
        }
    }
}
