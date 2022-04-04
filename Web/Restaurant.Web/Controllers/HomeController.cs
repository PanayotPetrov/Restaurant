namespace Restaurant.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Web.ViewModels;
    using Restaurant.Web.ViewModels.Review;

    public class HomeController : BaseController
    {
        private readonly IReviewService reviewService;

        public HomeController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        public IActionResult Index()
        {
            var latestReviews = this.reviewService.GetLatestFiveReviews<ReviewViewModel>();
            var model = new ReviewListViewModel { Reviews = latestReviews };
            return this.View(model);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult About()
        {
            return this.View();
        }

        public IActionResult Contact()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        public IActionResult StatusCodeError(string errorCode)
        {
            return this.View("StatusCodeError", errorCode);
        }
    }
}
