namespace Restaurant.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Web.Infrastructure.Filters;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Restaurant.Web.ViewModels.Order;

    public class OrderController : AdministrationController
    {
        private const int ItemsPerPage = 6;
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet("/Administration/Order/All/{id}")]
        public IActionResult Index(int id)
        {
            if (id < 1)
            {
                return this.NotFound();
            }

            var orders = this.orderService.GetAllWithDeleted<AdminOrderViewModel>(ItemsPerPage, id);

            var model = new AdminOrderListViewModel
            {
                Orders = orders,
                ItemsPerPage = ItemsPerPage,
                ItemCount = this.orderService.GetCountWithDeleted(),
                PageNumber = id,
            };

            if (id > model.PagesCount)
            {
                return this.NotFound();
            }

            this.TempData["RouteId"] = id;

            return this.View(model);
        }

        [HttpGet("/Administration/Order/Details/{orderNumber}")]
        [GetModelErrorsFromTempDataActionFilter]
        public IActionResult Details([OrderNumberValidation] string orderNumber)
        {
            if (!this.ModelState.IsValid)
            {
                if (!this.ModelState.Keys.Contains("Tempdata error"))
                {
                    return this.NotFound();
                }
            }

            var model = this.orderService.GetByOrderNumberWithDeleted<AdminOrderViewModel>(orderNumber);

            return this.View(model);
        }

        [HttpPost]
        [AddModelErrorsToTempDataActionFilter]
        public async Task<IActionResult> Delete([OrderNumberValidation] string orderNumber)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            var result = await this.orderService.DeleteByIdAsync(orderNumber);
            if (!result)
            {
                this.ModelState.AddModelError("Tempdata error", "This order has already been deleted!");
            }

            return this.RedirectToAction(nameof(this.Details), new { orderNumber });
        }

        [HttpPost]
        [AddModelErrorsToTempDataActionFilter]
        public async Task<IActionResult> Restore([OrderNumberValidation] string orderNumber)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            var result = await this.orderService.RestoreAsync(orderNumber);

            if (!result)
            {
                this.ModelState.AddModelError("Tempdata error", "Cannot restore an order which has not been deleted!");
            }

            return this.RedirectToAction(nameof(this.Details), new { orderNumber });
        }

        [HttpPost]
        [AddModelErrorsToTempDataActionFilter]
        public async Task<IActionResult> Complete([OrderNumberValidation] string orderNumber)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            var result = await this.orderService.CompleteAsync(orderNumber);

            if (!result)
            {
                this.ModelState.AddModelError("Tempdata error", "This order has already been completed!");
            }

            return this.RedirectToAction(nameof(this.Details), new { orderNumber });
        }
    }
}
