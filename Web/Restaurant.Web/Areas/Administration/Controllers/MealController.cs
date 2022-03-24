namespace Restaurant.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
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

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var model = this.mealService.GetByIdWithDeleted<AdminMealViewModel>((int)id);

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

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var model = this.mealService.GetByIdWithDeleted<EditMealInputModel>((int)id);

            if (model == null)
            {
                return this.NotFound();
            }

            model.Categories = this.categoryService.GetAllAsKeyValuePairs();
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditMealInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Categories = this.categoryService.GetAllAsKeyValuePairs();
                return this.View(model);
            }

            var editMealModel = AutoMapperConfig.MapperInstance.Map<EditMealModel>(model);

            await this.mealService.UpdateAsync(editMealModel, $"{this.environment.WebRootPath}/images");
            return this.RedirectToAction(nameof(this.Details), new { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await this.mealService.DeleteByIdAsync(id);
                return this.RedirectToAction(nameof(this.Details), new { Id = id });
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                var model = this.mealService.GetByIdWithDeleted<EditMealInputModel>(id);
                model.Categories = this.categoryService.GetAllAsKeyValuePairs();
                return this.View("Edit", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            try
            {
                await this.mealService.RestoreAsync(id);
                return this.RedirectToAction(nameof(this.Details), new { Id = id });
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                var model = this.mealService.GetByIdWithDeleted<EditMealInputModel>(id);
                model.Categories = this.categoryService.GetAllAsKeyValuePairs();
                return this.View("Edit", model);
            }
        }
    }
}
