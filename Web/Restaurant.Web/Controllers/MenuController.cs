﻿namespace Restaurant.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using Restaurant.Services.Data;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels.Category;
    using Restaurant.Web.ViewModels.InputModels;
    using Restaurant.Web.ViewModels.Meal;
    using Restaurant.Web.ViewModels.Menu;

    public class MenuController : BaseController
    {
        private readonly IMealService mealService;
        private readonly ICategoryService categoryService;
        private readonly IWebHostEnvironment environment;

        public MenuController(IMealService mealService, ICategoryService categoryService, IWebHostEnvironment environment)
        {
            this.mealService = mealService;
            this.categoryService = categoryService;
            this.environment = environment;
        }

        [HttpGet("/Menu/")]
        public IActionResult AllMeals()
        {
            var menu = new MenuViewModel { Meals = this.mealService.GetAllMeals<MealViewModel>(), Categories = this.categoryService.GetAll<CategoryViewModel>() };

            return this.View(menu);
        }

        // [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult AddMeal()
        {
            var viewModel = new AddMealInputModel
            {
                CategoryItems = this.categoryService.GetAllAsKeyValuePairs(),
            };
            return this.View(viewModel);
        }

        // [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [HttpPost]
        public async Task<IActionResult> AddMeal(AddMealInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.CategoryItems = this.categoryService.GetAllAsKeyValuePairs();
                return this.View(model);
            }

            var addMealModel = new AddMealModel
            {
                CategoryId = model.CategoryId,
                Image = model.Image,
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
            };

            await this.mealService.CreateAsync(addMealModel, $"{this.environment.WebRootPath}/images");

            return this.RedirectToAction(nameof(this.AllMeals));
        }
    }
}
