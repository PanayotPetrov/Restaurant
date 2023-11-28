namespace Restaurant.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels;
    using Restaurant.Web.ViewModels.Home;
    using Restaurant.Web.ViewModels.InputModels;
    using Restaurant.Web.ViewModels.Review;

    public class HomeController : BaseController
    {
        private readonly IReviewService reviewService;
        private readonly IUserMessageService userMessageService;

        public HomeController(IReviewService reviewService, IUserMessageService userMessageService)
        {
            this.reviewService = reviewService;
            this.userMessageService = userMessageService;
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
            var categories = this.userMessageService.GetCategories<UserMessageCategoryViewModel>();
            var model = new UserMessageInputModel
            {
                Categories = categories,
            };
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Contact(UserMessageInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Categories = this.userMessageService.GetCategories<UserMessageCategoryViewModel>();
                return this.View(model);
            }

            if (this.User.Identity.IsAuthenticated)
            {
                model.ApplicationUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var addUserMessageModel = AutoMapperConfig.MapperInstance.Map<AddUserMessageModel>(model);
            var userMessageId = await this.userMessageService.CreateAsync(addUserMessageModel);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public IActionResult SetLanguage(string changeCultureString, string returnUrl)
        {
            this.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(changeCultureString)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return this.LocalRedirect(returnUrl);
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
