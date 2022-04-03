namespace Restaurant.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
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

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var model = this.orderService.GetByIdWithDeleted<AdminOrderViewModel>((int)id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.orderService.DeleteByIdAsync(id);
            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, "This order has already been deleted!");
                var order = this.orderService.GetByIdWithDeleted<AdminOrderViewModel>(id);
                return this.View("Details", order);
            }

            return this.RedirectToAction(nameof(this.Details), new { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            var result = await this.orderService.RestoreAsync(id);

            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, "Cannot restore an order which has not been deleted!");
                var order = this.orderService.GetByIdWithDeleted<AdminOrderViewModel>(id);
                return this.View("Details", order);
            }

            return this.RedirectToAction(nameof(this.Details), new { Id = id });
        }
    }
}
