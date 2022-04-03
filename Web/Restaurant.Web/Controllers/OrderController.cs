namespace Restaurant.Web.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.Infrastructure.Filters;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Restaurant.Web.ViewModels.InputModels;
    using Restaurant.Web.ViewModels.Order;

    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        [AddModelErrorsToTempDataActionFilter]
        public async Task<IActionResult> Create(CreateOrderInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Checkout/Index");
            }

            var addOrderModel = AutoMapperConfig.MapperInstance.Map<AddOrderModel>(model);
            addOrderModel.ApplicationUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var orderNumber = await this.orderService.CreateAsync(addOrderModel);
            return this.RedirectToAction(nameof(this.AllOrders), new { orderNumber });
        }

        [HttpGet("/Order/All/{orderNumber?}")]
        [AddOrderRouteIdActionFilter]
        public IActionResult AllOrders([OrderNumberValidation]string orderNumber)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.UrlNotFound));
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var model = this.orderService.GetByUserIdAndOrderNumber<AllOrdersViewModel>(userId, orderNumber);
            return this.View(model);
        }

        [HttpGet("/Order/Success/{orderNumber?}")]
        public IActionResult Success([OrderNumberValidation][Required] string orderNumber)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.UrlNotFound));
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var model = this.orderService.GetByUserIdAndOrderNumber<OrderViewModel>(userId, orderNumber);
            return this.View(model);
        }
    }
}
