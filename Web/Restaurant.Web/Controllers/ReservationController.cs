namespace Restaurant.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Restaurant.Web.ViewModels.InputModels;
    using Restaurant.Web.ViewModels.Reservation;

    public class ReservationController : BaseController
    {
        private readonly IReservationService reservationService;

        public ReservationController(IReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        [Authorize]
        public IActionResult AllReservations()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            return this.View();
        }

        public IActionResult Book()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Book(AddReservationInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            if (this.User.Identity.IsAuthenticated)
            {
                model.UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            try
            {
                model.ReservationDate = model.ReservationDate.AddHours(model.ReservationTime);
                var reservationId = await this.reservationService.CreateReservationAsync(model);
                return this.Redirect(nameof(this.Success) + $"?reservationId={reservationId}");
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(model);
            }
        }

        public IActionResult Success(string reservationId)
        {
            var model = this.reservationService.GetById<ReservationViewModel>(reservationId);
            return this.View(model);
        }
    }
}
