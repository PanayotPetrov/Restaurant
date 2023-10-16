namespace Restaurant.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.Infrastructure.Filters;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Restaurant.Web.ViewModels.InputModels;
    using Restaurant.Web.ViewModels.Meal;
    using Restaurant.Web.ViewModels.Paging.PagedItemsModelCreator;

    public class MealController : AdministrationController
    {
        private const int ItemsPerPage = 6;
        private readonly IMealService mealService;
        private readonly ICategoryService categoryService;
        private readonly IWebHostEnvironment environment;
        private readonly IPagedItemsModelCreator modelCreator;

        public MealController(IMealService mealService, ICategoryService categoryService, IWebHostEnvironment environment, IPagedItemsModelCreator modelCreator)
        {
            this.mealService = mealService;
            this.categoryService = categoryService;
            this.environment = environment;
            this.modelCreator = modelCreator;
        }

        [HttpGet("/Administration/Meal/All/{id}")]
        public IActionResult Index(int id)
        {
            var totalItemsCount = this.mealService.GetCount(true);
            var items = this.mealService.GetAllWithPagination<AdminMealViewModel>(ItemsPerPage, id, true);
            var model = this.modelCreator.Create(items, id, ItemsPerPage, totalItemsCount);

            if (!model.HasValidState)
            {
                return this.NotFound();
            }

            this.TempData["RouteId"] = id;

            return this.View(model);
        }

        [GetModelErrorsFromTempDataActionFilter]
        public IActionResult Details([MealIdValidation] int id)
        {
            if (!this.ModelState.IsValid)
            {
                if (!this.ModelState.Keys.Contains("Tempdata error"))
                {
                    return this.NotFound();
                }
            }

            var model = this.mealService.GetById<AdminMealViewModel>(id, true);
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

            var model = this.mealService.GetById<EditMealInputModel>(id, true);
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
            return this.RedirectToAction(nameof(this.Details), new { model.Id });
        }

        [HttpPost]
        [AddModelErrorsToTempDataActionFilter]
        public async Task<IActionResult> Delete([MealIdValidation] int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            var result = await this.mealService.DeleteByIdAsync(id);

            if (!result)
            {
                this.ModelState.AddModelError("Tempdata error", "This meal has already been deleted!");
            }

            return this.RedirectToAction(nameof(this.Details), new { Id = id });
        }

        [HttpPost]
        [AddModelErrorsToTempDataActionFilter]
        public async Task<IActionResult> Restore([MealIdValidation] int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            var result = await this.mealService.RestoreAsync(id);

            if (!result)
            {
                this.ModelState.AddModelError("Tempdata error", "Cannot restore a meal which has not been deleted!");
            }

            return this.RedirectToAction(nameof(this.Details), new { Id = id });
        }
    }
}
