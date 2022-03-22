using Microsoft.AspNetCore.Mvc;

namespace Restaurant.Web.Areas.Administration.Controllers
{
    public class DashboardController : AdministrationController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
