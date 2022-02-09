namespace Restaurant.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ReviewController : BaseController
    {
        public ReviewController()
        {
        }

        public IActionResult Add()
        {
            return this.View();
        }

        public IActionResult All()
        {
            return this.View();
        }
    }
}
