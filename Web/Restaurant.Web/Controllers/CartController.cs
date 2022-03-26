namespace Restaurant.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels.Cart;

    public class CartController : BaseController
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var model = this.cartService.GetCartByUserId<CartViewModel>(userId);

            if (model is null)
            {
                model = await this.cartService.CreateCartForUserAsync<CartViewModel>(userId);
            }

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult AddToCart(CartItemModel model)
        {
            //var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //if (!this.ModelState.IsValid)
            //{
            //    return this.ValidationProblem();
            //}

            //var cartItem = AutoMapperConfig.MapperInstance.Map<CartItemModel>(model);

            //this.cartService.AddToCartAsync(userId, cartItem);
            return this.Ok();
        }

        [Authorize]
        [HttpPost]
        public IActionResult RemoveFromCart(CartItemModel model)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem();
            }

            var cartItem = AutoMapperConfig.MapperInstance.Map<CartItemModel>(model);

            this.cartService.RemoveFromCartAsync(userId, cartItem);
            return this.Ok();
        }
    }
}
