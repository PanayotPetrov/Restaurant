namespace Restaurant.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Restaurant.Web.ViewModels.InputModels;
    using Restaurant.Web.ViewModels.Meal;

    public class MealController : AdministrationController
    {
        private const int ItemsPerPage = 6;
        private readonly IMealService mealService;
        private readonly ICategoryService categoryService;
        private readonly IWebHostEnvironment environment;

        public MealController(IMealService mealService, ICategoryService categoryService, IWebHostEnvironment environment)
        {
            this.mealService = mealService;
            this.categoryService = categoryService;
            this.environment = environment;
        }

        [HttpGet("/Administration/Meal/All/{id}")]
        public IActionResult Index(int id)
        {
            if (id < 1)
            {
                return this.NotFound();
            }

            var model = new AdminMealListViewModel
            {
                Meals = this.mealService.GetAllWithPagination<AdminMealViewModel>(ItemsPerPage, id),
                ItemsPerPage = ItemsPerPage,
                ItemCount = this.mealService.GetMealCount(),
                PageNumber = id,
            };

            if (id > model.PagesCount)
            {
                return this.NotFound();
            }

            this.TempData["RouteId"] = id;

            return this.View(model);
        }

        public IActionResult Details([MealIdValidation] int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            var model = this.mealService.GetByIdWithDeleted<AdminMealViewModel>(id);
            return this.View(model);
        }

        public IActionResult Create()
        {
            var viewModel = new AddMealInputModel
            {
                Categories = this.categoryService.GetAllAsKeyValuePairs(),
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddMealInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Categories = this.categoryService.GetAllAsKeyValuePairs();
                return this.View(model);
            }

            var addMealModel = AutoMapperConfig.MapperInstance.Map<AddMealModel>(model);

            var id = await this.mealService.CreateAsync(addMealModel, $"{this.environment.WebRootPath}/images");
            return this.RedirectToAction(nameof(this.Details), new { Id = id });
        }

        public IActionResult Edit([MealIdValidation] int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            var model = this.mealService.GetByIdWithDeleted<EditMealInputModel>(id);
            model.Categories = this.categoryService.GetAllAsKeyValuePairs();
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMealInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Categories = this.categoryService.GetAllAsKeyValuePairs();
                return this.View(model);
            }

            var editMealModel = AutoMapperConfig.MapperInstance.Map<EditMealModel>(model);

            await this.mealService.UpdateAsync(editMealModel, $"{this.environment.WebRootPath}/images");
            return this.RedirectToAction(nameof(this.Details), new { Id = model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete([MealIdValidation] int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            var result = await this.mealService.DeleteByIdAsync(id);

            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, "This meal has already been deleted!");
                var model = this.mealService.GetByIdWithDeleted<EditMealInputModel>(id);
                model.Categories = this.categoryService.GetAllAsKeyValuePairs();
                return this.View("Edit", model);
            }

            return this.RedirectToAction(nameof(this.Details), new { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Restore([MealIdValidation] int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            var result = await this.mealService.RestoreAsync(id);

            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, "Cannot restore a meal which has not been deleted!");
                var model = this.mealService.GetByIdWithDeleted<EditMealInputModel>(id);
                model.Categories = this.categoryService.GetAllAsKeyValuePairs();
                return this.View("Edit", model);
            }

            return this.RedirectToAction(nameof(this.Details), new { Id = id });
        }
    }
}
