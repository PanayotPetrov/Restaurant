namespace Restaurant.Web.Controllers
{

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;

    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        public IActionResult Create(int id)
        {
            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            return this.View();
        }
    }
}
