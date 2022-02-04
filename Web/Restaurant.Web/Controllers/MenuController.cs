namespace Restaurant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Web.ViewModels.Meal;
    using Restaurant.Web.ViewModels.Menu;

    public class MenuController : BaseController
    {
        private readonly IMenuService menuService;

        public MenuController(IMenuService menuService)
        {
            this.menuService = menuService;
        }

        [HttpGet("/Menu/")]
        public IActionResult All()
        {
            var menu = new MenuViewModel { Meals = this.menuService.GetAllMeals<MealViewModel>() };

            return this.View(menu);
        }

        // TO DO: Add one more view - (show by categories?)
    }
}
