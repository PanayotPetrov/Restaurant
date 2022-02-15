namespace Restaurant.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Restaurant.Data.Models;
    using Restaurant.Services.Data;
    using Restaurant.Web.ViewModels.InputModels;
    using Restaurant.Web.ViewModels.Review;

    public class ReviewController : BaseController
    {
        private readonly IReviewService reviewService;
        private readonly UserManager<ApplicationUser> userManager;

        public ReviewController(IReviewService reviewService, UserManager<ApplicationUser> userManager)
        {
            this.reviewService = reviewService;
            this.userManager = userManager;
        }

        public IActionResult All()
        {
            var reviews = this.reviewService.GetAllReviews<ReviewViewModel>();
            var model = new ReviewListViewModel
            {
                Reviews = reviews,
            };

            return this.View(model);
        }

        [Authorize]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddReviewInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var userId = this.userManager.GetUserId(this.User);
            model.ApplicationUserId = userId;
            await this.reviewService.AddReviewAsync(model);
            return this.RedirectToAction(nameof(this.All));
        }
    }
}
