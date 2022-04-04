namespace Restaurant.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
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

            var reviews = this.reviewService.GetAllWithDeleted<AdminReviewViewModel>(ItemsPerPage, id);

            var model = new AdminReviewListViewModel
            {
                Reviews = reviews,
                ItemsPerPage = ItemsPerPage,
                ItemCount = this.reviewService.GetCountWithDeleted(),
                PageNumber = id,
            };

            if (id > model.PagesCount)
            {
                return this.NotFound();
            }

            this.TempData["RouteId"] = id;

            return this.View(model);
        }

        public IActionResult Details(int id)
        {
            var model = this.reviewService.GetByIdWithDeleted<AdminReviewViewModel>(id);

            if (model is null)
            {
                return this.NotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.reviewService.DeleteByIdAsync(id);
            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, "This review has already been deleted!");
                var review = this.reviewService.GetByIdWithDeleted<AdminReviewViewModel>(id);
                return this.View("Details", review);
            }

            return this.RedirectToAction(nameof(this.Details), new { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            var result = await this.reviewService.RestoreAsync(id);

            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, "Cannot restore a review which has not been deleted!");
                var review = this.reviewService.GetByIdWithDeleted<AdminReviewViewModel>(id);
                return this.View("Details", review);
            }

            return this.RedirectToAction(nameof(this.Details), new { Id = id });
        }
    }
}
