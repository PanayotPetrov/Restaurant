namespace AdminDashboard.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Web.Infrastructure.Filters;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Restaurant.Web.ViewModels.Order;
    using Restaurant.Web.ViewModels.Paging.PagedItemsModelCreator;

    [Authorize]
    public class OrderController : Controller
    {
        private const int ItemsPerPage = 6;
        private readonly IOrderService orderService;
        private readonly IPagedItemsModelCreator modelCreator;

        public OrderController(IOrderService orderService, IPagedItemsModelCreator modelCreator)
        {
            this.orderService = orderService;
            this.modelCreator = modelCreator;
        }

        [HttpGet("Order/All/{id}")]
        public IActionResult Index(int id)
        {
            var totalItemsCount = this.orderService.GetCount(true);
            var items = this.orderService.GetAllWithPagination<AdminOrderViewModel>(ItemsPerPage, id, true);
            var model = this.modelCreator.Create(items, id, ItemsPerPage, totalItemsCount);

            if (!model.HasValidState)
            {
                return this.NotFound();
            }

            this.TempData["RouteId"] = id;

            return this.View(model);
        }

        [HttpGet("Order/Details/{orderNumber}")]
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

            var model = this.orderService.GetByOrderNumber<AdminOrderViewModel>(orderNumber, true);

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

            var result = await this.orderService.DeleteByOrderNumberAsync(orderNumber);
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
