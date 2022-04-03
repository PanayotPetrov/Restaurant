namespace Restaurant.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
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
        [HttpGet("/Reservation/All")]

        public IActionResult AllReservations()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var model = new ReservationListViewModel
            {
                Reservations = this.reservationService.GetAllByUserId<ReservationViewModel>(userId),
            };

            return this.View(model);
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
                model.ApplicationUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var addReservationModel = AutoMapperConfig.MapperInstance.Map<AddReservationModel>(model);
            var reservationId = await this.reservationService.CreateReservationAsync(addReservationModel);

            if (reservationId is null)
            {
                this.ModelState.AddModelError(string.Empty, $"We don't have a table for {model.NumberOfPeople} on the {model.ReservationDate}");
                return this.View(model);
            }

            return this.Redirect(nameof(this.Success) + $"?reservationId={reservationId}");
        }

        public IActionResult Success(string reservationId)
        {
            var model = this.reservationService.GetById<ReservationViewModel>(reservationId);
            return this.View(model);
        }
    }
}
