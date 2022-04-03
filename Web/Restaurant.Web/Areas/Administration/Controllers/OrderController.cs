namespace Restaurant.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Web.Infrastructure.Filters;
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
                return this.UrlNotFound();
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
                return this.UrlNotFound();
            }

            this.TempData["RouteId"] = id;

            return this.View(model);
        }

        [GetModelErrorsFromTempDataActionFilter]
        public IActionResult Details(int? id)
        {
            if (id == null || id <= 0)
            {
                return this.UrlNotFound();
            }

            var model = this.orderService.GetByIdWithDeleted<AdminOrderViewModel>((int)id);

            return this.View(model);
        }

        [HttpPost]
        [AddModelErrorsToTempDataActionFilter]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.orderService.DeleteByIdAsync(id);
            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, "This order has already been deleted!");
            }

            return this.RedirectToAction(nameof(this.Details), new { Id = id });
        }

        [HttpPost]
        [AddModelErrorsToTempDataActionFilter]
        public async Task<IActionResult> Restore(int id)
        {
            var result = await this.orderService.RestoreAsync(id);

            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, "Cannot restore an order which has not been deleted!");
            }

            return this.RedirectToAction(nameof(this.Details), new { Id = id });
        }

        [HttpPost]
        [AddModelErrorsToTempDataActionFilter]
        public async Task<IActionResult> Complete(int id)
        {
            var result = await this.orderService.CompleteAsync(id);
            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, "This order has already been completed!");
            }

            return this.RedirectToAction(nameof(this.Details), new { Id = id });
        }
    }
}
