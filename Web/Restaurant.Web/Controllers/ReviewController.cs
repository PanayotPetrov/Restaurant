namespace Restaurant.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Restaurant.Services.Data;
    using Restaurant.Web.ViewModels.InputModels;
    using Restaurant.Web.ViewModels.Review;

    public class ReviewController : BaseController
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpGet("/Review/All")]
        public IActionResult AllReviews()
        {
            var reviews = this.reviewService.GetAllReviews<ReviewViewModel>();
            var model = new ReviewListViewModel
            {
                Reviews = reviews,
            };

            return this.View(model);
        }

        [Authorize]
        [HttpGet("/Review/Add")]
        public IActionResult AddReview()
        {
            return this.View();
        }

        [HttpPost("/Review/Add")]
        [Authorize]
        public async Task<IActionResult> AddReview(AddReviewInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            model.ApplicationUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.reviewService.AddReviewAsync(model);
            return this.RedirectToAction(nameof(this.AllReviews));
        }
    }
}
