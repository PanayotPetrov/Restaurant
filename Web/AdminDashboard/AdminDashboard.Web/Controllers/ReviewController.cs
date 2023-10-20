namespace AdminDashboard.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Web.Infrastructure.Filters;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Restaurant.Web.ViewModels.Paging.PagedItemsModelCreator;
    using Restaurant.Web.ViewModels.Review;

    [Authorize]
    public class ReviewController : Controller
    {
        private const int ItemsPerPage = 6;
        private readonly IReviewService reviewService;
        private readonly IPagedItemsModelCreator modelCreator;

        public ReviewController(IReviewService reviewService, IPagedItemsModelCreator modelCreator)
        {
            this.reviewService = reviewService;
            this.modelCreator = modelCreator;
        }

        [HttpGet("/Review/All/{id}")]
        public IActionResult Index(int id)
        {
            var totalItemsCount = this.reviewService.GetCount(true);
            var items = this.reviewService.GetAllWithPagination<AdminReviewViewModel>(ItemsPerPage, id, true);
            var model = this.modelCreator.Create(items, id, ItemsPerPage, totalItemsCount);

            if (!model.HasValidState)
            {
                return this.NotFound();
            }

            this.TempData["RouteId"] = model.PageNumber;
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
