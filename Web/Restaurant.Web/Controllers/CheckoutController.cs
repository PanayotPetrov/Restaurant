namespace Restaurant.Web.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Web.Infrastructure.Filters;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Restaurant.Web.ViewModels.Address;
    using Restaurant.Web.ViewModels.Checkout;

    [Authorize]
    public class CheckoutController : BaseController
    {
        private readonly ICartService cartService;
        private readonly IAddressService addressService;

        public CheckoutController(ICartService cartService, IAddressService addressService)
        {
            this.cartService = cartService;
            this.addressService = addressService;
        }

        public IActionResult Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var model = this.cartService.GetCartByUserId<CheckoutViewModel>(userId);
            return this.View(model);
        }

        [ReturnModelStateErrorsAsJsonActionFilter]
        [IgnoreAntiforgeryToken]

        public IActionResult GetAddressDetails([AddressNameValidation][FromQuery] string addressName)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var model = this.addressService.GetByUserIdAndAddressName<AddressViewModel>(userId, addressName);
            this.Response.StatusCode = 200;
            return this.Json(model);
        }
    }
}
