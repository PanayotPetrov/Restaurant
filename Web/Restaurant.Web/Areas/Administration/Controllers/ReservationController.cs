namespace Restaurant.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels.InputModels;
    using Restaurant.Web.ViewModels.Reservation;

    public class ReservationController : AdministrationController
    {
        private const int ItemsPerPage = 6;
        private readonly IReservationService reservationService;

        public ReservationController(IReservationService reviewService)
        {
            this.reservationService = reviewService;
        }

        [HttpGet("/Administration/Reservation/All/{id}")]
        public IActionResult Index(int id)
        {
            if (id < 1)
            {
                return this.NotFound();
            }

            var reservations = this.reservationService.GetAllAfterCurrentDate<AdminReservationViewModel>(ItemsPerPage, id);

            var model = new AdminReservationListViewModel
            {
                Reservations = reservations,
                ItemsPerPage = ItemsPerPage,
                ItemCount = this.reservationService.GetCount(),
                PageNumber = id,
            };

            if (id > model.PagesCount)
            {
                return this.NotFound();
            }

            this.TempData["RouteId"] = id;

            return this.View(model);
        }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var model = this.reservationService.GetByIdWithDeleted<AdminReservationViewModel>(id);

            return this.View(model);
        }

        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var model = this.reservationService.GetByIdWithDeleted<EditReservationInputModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditReservationInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            model.ReservationDate = model.ReservationDate.AddHours(model.ReservationTime);
            var addReservationModel = AutoMapperConfig.MapperInstance.Map<EditReservationModel>(model);
            var result = await this.reservationService.UpdateAsync(addReservationModel);

            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, $"We don't have a table for {model.NumberOfPeople} on the {model.ReservationDate}");
                return this.View(model);
            }

            return this.RedirectToAction(nameof(this.Details), new { model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await this.reservationService.DeleteByIdAsync(id);

            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, "This reservation has already been deleted!");
                var review = this.reservationService.GetByIdWithDeleted<EditReservationInputModel>(id);
                return this.View("Edit", review);
            }

            return this.RedirectToAction(nameof(this.Details), new { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Restore(string id)
        {
            var result = await this.reservationService.RestoreAsync(id);
            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, "Cannot restore a reservation which has not been deleted!");
                var reservation = this.reservationService.GetById<EditReservationInputModel>(id);
                return this.View("Edit", reservation);
            }

            return this.RedirectToAction(nameof(this.Details), new { Id = id });
        }
    }
}
