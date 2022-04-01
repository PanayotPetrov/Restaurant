namespace Restaurant.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.Infrastructure.Filters;
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
            return this.RedirectToAction(nameof(this.Success), new { orderNumber });
        }

        public IActionResult All()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var orders = this.orderService.GetAllByUserId<OrderViewModel>(userId);
            var model = new OrderInListViewModel { Orders = orders };
            return this.View(model);
        }

        public IActionResult Success(string orderNumber)
        {
            var model = this.orderService.GetByOrderNumber<OrderViewModel>(orderNumber);
            return this.View(model);
        }
    }
}
