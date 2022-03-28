namespace Restaurant.Web.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels.Cart;
    using Restaurant.Web.ViewModels.InputModels;

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
        public async Task<IActionResult> AddToCart([FromBody]AddToCartInputModel model)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!this.ModelState.IsValid)
            {
                var modelErrors = new List<string>();

                foreach (var modelState in this.ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }

                this.Response.StatusCode = 400;
                return this.Json(modelErrors);
            }

            var cartItem = AutoMapperConfig.MapperInstance.Map<CartItemModel>(model);

            await this.cartService.AddToCartAsync(userId, cartItem);
            var cartPrice = this.cartService.GetCartTotalPrice(userId);
            this.Response.StatusCode = 200;
            return this.Json(cartPrice.ToString("0.00"));
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
