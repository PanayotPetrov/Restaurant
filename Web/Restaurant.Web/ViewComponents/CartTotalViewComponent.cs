namespace Restaurant.Web.ViewComponents
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;

    public class CartTotalViewComponent : ViewComponent
    {
        private readonly ICartService cartService;

        public CartTotalViewComponent(ICartService cartService)
        {
            this.cartService = cartService;
        }

        public IViewComponentResult Invoke()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                this.ViewBag.CartPrice = this.cartService.GetCartSubTotal(userId).ToString("0.00");
            }

            return this.View();
        }
    }
}
