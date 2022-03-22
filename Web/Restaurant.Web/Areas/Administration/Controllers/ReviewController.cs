namespace Restaurant.Web.Areas.Administration.Controllers
{
    using System;
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

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var model = this.reviewService.GetById<AdminReviewViewModel>((int)id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await this.reviewService.DeleteByIdAsync(id);
                return this.RedirectToAction(nameof(this.Index), new { Id = 1 });
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                var review = this.reviewService.GetById<AdminReviewViewModel>(id);
                return this.View("Details", review);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            try
            {
                await this.reviewService.RestoreAsync(id);
                return this.RedirectToAction(nameof(this.Details), new { Id = id });
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                var review = this.reviewService.GetById<AdminReviewViewModel>(id);
                return this.View("Details", review);
            }
        }
    }
}
