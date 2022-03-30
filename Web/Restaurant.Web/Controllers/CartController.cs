﻿namespace Restaurant.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.Infrastructure.Filters;
    using Restaurant.Web.ViewModels.Cart;
    using Restaurant.Web.ViewModels.InputModels;

    [Authorize]
    public class CartController : BaseController
    {
        private readonly ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

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

        [HttpPost]
        [IgnoreAntiforgeryToken]
        [ReturnModelStateErrorsAsJsonActionFilter]
        public async Task<IActionResult> AddToCart([FromBody]CartInputModel model)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cartItem = AutoMapperConfig.MapperInstance.Map<CartItemModel>(model);

            await this.cartService.AddToCartAsync(userId, cartItem);
            var cartSubtotal = this.cartService.GetCartSubTotal(userId);
            this.Response.StatusCode = 200;
            return this.Json(cartSubtotal.ToString("0.00"));
        }

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

        [HttpPost]
        [IgnoreAntiforgeryToken]
        [ReturnModelStateErrorsAsJsonActionFilter]
        public async Task<IActionResult> ChangeQuantity([FromBody] CartInputModel model)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cartItem = AutoMapperConfig.MapperInstance.Map<CartItemModel>(model);

            var jsonModel = await this.cartService.ChangeItemQuantityAsync<ChangeItemQuantityViewModel>(userId, cartItem);
            this.Response.StatusCode = 200;
            return this.Json(jsonModel);
        }
    }
}