namespace Restaurant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Web.ViewModels.InputModels;

    public class ReservationController : BaseController
    {
        public IActionResult Book()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Book(AddReservationInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            // TO DO: Add reservation to DB.
            return this.RedirectToAction(nameof(this.Success));
        }

        public IActionResult Success()
        {
            return this.View();
        }
    }
}
