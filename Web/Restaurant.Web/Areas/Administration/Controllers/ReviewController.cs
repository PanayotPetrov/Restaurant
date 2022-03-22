namespace Restaurant.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
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
                ItemCount = this.reviewService.GetCount(),
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

        public IActionResult Delete(int id)
        {
            var review = this.reviewService.GetById<AdminReviewViewModel>(id);
            return this.View(review);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
                return this.View(review);
            }
        }
    }
}
