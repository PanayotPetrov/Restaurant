namespace Restaurant.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : Controller
    {
        public IActionResult UrlNotFound()
        {
            return this.View();
        }
    }
}
