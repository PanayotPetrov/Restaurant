namespace Restaurant.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using Restaurant.Services.Data;
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
        public IActionResult All()
        {
            var menu = new MenuViewModel { Meals = this.mealService.GetAllMeals<MealViewModel>() };

            return this.View(menu);
        }

        // [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Add()
        {
            var viewModel = new AddMealInputModel
            {
                CategoryItems = this.categoryService.GetAllAsKeyValuePairs(),
            };
            return this.View(viewModel);
        }

        // [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [HttpPost]
        public async Task<IActionResult> Add(AddMealInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.CategoryItems = this.categoryService.GetAllAsKeyValuePairs();
                return this.View(model);
            }

            await this.mealService.CreateAsync(model, $"{this.environment.WebRootPath}/images");

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
