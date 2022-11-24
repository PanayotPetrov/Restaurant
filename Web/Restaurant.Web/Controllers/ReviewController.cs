namespace Restaurant.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.HelperClasses;
    using Restaurant.Web.ViewModels.InputModels;
    using Restaurant.Web.ViewModels.Review;

    public class ReviewController : BaseController
    {
        private const int ItemsPerPage = 4;
        private readonly IReviewService reviewService;
        private readonly IPagedItemsModelCreator modelCreator;

        public ReviewController(IReviewService reviewService, IPagedItemsModelCreator modelCreator)
        {
            this.reviewService = reviewService;
            this.modelCreator = modelCreator;
        }

        [HttpGet("/Review/All/{id}")]
        public IActionResult AllReviews(int id)
        {
            var totalItemsCount = this.reviewService.GetCount();
            var items = this.reviewService.GetAllWithPagination<ReviewViewModel>(ItemsPerPage, id);
            var model = this.modelCreator.Create(items, id, ItemsPerPage, totalItemsCount);

            if (!model.HasValidState)
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
