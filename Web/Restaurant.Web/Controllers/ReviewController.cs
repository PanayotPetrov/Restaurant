namespace Restaurant.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels.InputModels;
    using Restaurant.Web.ViewModels.Review;

    public class ReviewController : BaseController
    {
        private const int ItemsPerPage = 4;
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpGet("/Review/All/{id}")]
        public IActionResult AllReviews(int id)
        {
            if (id < 1)
            {
                return this.NotFound();
            }

            var reviews = this.reviewService.GetAllReviews<ReviewViewModel>(ItemsPerPage, id);

            var model = new ReviewListViewModel
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

            return this.View(model);
        }

        [Authorize]
        [HttpGet("/Review/Add")]
        public IActionResult AddReview()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost("/Review/Add")]
        public async Task<IActionResult> AddReview(AddReviewInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            model.ApplicationUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var addReviewModel = AutoMapperConfig.MapperInstance.Map<AddReviewModel>(model);

            await this.reviewService.AddReviewAsync(addReviewModel);
            return this.Redirect($"/Review/All/1");
        }
    }
}
