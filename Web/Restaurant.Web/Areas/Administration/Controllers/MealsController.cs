namespace Restaurant.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using Restaurant.Data;
    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels.InputModels;
    using Restaurant.Web.ViewModels.Meal;

    public class MealsController : AdministrationController
    {
        private const int ItemsPerPage = 6;
        private readonly IMealService mealService;
        private readonly ICategoryService categoryService;
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;

        public MealsController(IMealService mealService, ICategoryService categoryService, IWebHostEnvironment environment, ApplicationDbContext context)
        {
            this.mealService = mealService;
            this.categoryService = categoryService;
            this.environment = environment;
        }

        [HttpGet("/Administration/Meals/All/{id}")]
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

            await this.mealService.CreateAsync(addMealModel, $"{this.environment.WebRootPath}/images");

            return this.RedirectToAction(nameof(this.Index), new { Id = 1 });
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var model = this.mealService.GetById<EditMealInputModel>((int)id);

            if (model == null)
            {
                return this.NotFound();
            }

            model.Categories = this.categoryService.GetAllAsKeyValuePairs();
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditMealInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Categories = this.categoryService.GetAllAsKeyValuePairs();
                return this.View(model);
            }

            var addMealModel = AutoMapperConfig.MapperInstance.Map<AddMealModel>(model);

            await this.mealService.UpdateAsync(addMealModel, model.Id, $"{this.environment.WebRootPath}/images");
            return this.RedirectToAction(nameof(this.Index), new { Id = 1 });
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var meal = this.mealService.GetById<AdminMealViewModel>((int)id);
            if (meal == null)
            {
                return this.NotFound();
            }

            return this.View(meal);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this.mealService.DeleteById(id);
            return this.RedirectToAction(nameof(this.Index), new { Id = 1 });
        }
    }
}
