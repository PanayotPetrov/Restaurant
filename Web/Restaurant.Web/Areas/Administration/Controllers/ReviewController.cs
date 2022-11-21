namespace Restaurant.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Web.Infrastructure.Filters;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Restaurant.Web.ViewModels.Review;

    public class ReviewController : AdministrationController
    {
        private const int ItemsPerPage = 6;
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpGet("/Administration/Review/All/{id}")]
        public IActionResult Index(int id)
        {
            if (id < 1)
            {
                return this.NotFound();
            }

            var reviews = this.reviewService.GetAllWithPagination<AdminReviewViewModel>(ItemsPerPage, id, true);

            var model = new AdminReviewListViewModel
            {
                Reviews = reviews,
                ItemsPerPage = ItemsPerPage,
                ItemCount = this.reviewService.GetCount(true),
                PageNumber = id,
            };

            if (id > model.PagesCount)
            {
                return this.NotFound();
            }

            this.TempData["RouteId"] = id;

            return this.View(model);
        }

        [GetModelErrorsFromTempDataActionFilter]
        public IActionResult Details([ReviewIdValidation] int id)
        {
            if (!this.ModelState.IsValid)
            {
                if (!this.ModelState.Keys.Contains("Tempdata error"))
                {
                    return this.NotFound();
                }
            }

            var model = this.reviewService.GetById<AdminReviewViewModel>(id, true);

            return this.View(model);
        }

        [HttpPost]
        [AddModelErrorsToTempDataActionFilter]
        public async Task<IActionResult> Delete([ReviewIdValidation] int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            var result = await this.reviewService.DeleteByIdAsync(id);

            if (!result)
            {
                this.ModelState.AddModelError("Tempdata error", "This review has already been deleted!");
            }

            return this.RedirectToAction(nameof(this.Details), new { id });
        }

        [HttpPost]
        [AddModelErrorsToTempDataActionFilter]

        public async Task<IActionResult> Restore([ReviewIdValidation] int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            var result = await this.reviewService.RestoreAsync(id);

            if (!result)
            {
                this.ModelState.AddModelError("Tempdata error", "Cannot restore a review which has not been deleted!");
            }

            return this.RedirectToAction(nameof(this.Details), new { id });
        }
    }
}
