namespace Restaurant.Web.Areas.Administration.Controllers
{
    using System;
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

            try
            {
                model.ReservationDate = model.ReservationDate.AddHours(model.ReservationTime);
                var addReservationModel = AutoMapperConfig.MapperInstance.Map<EditReservationModel>(model);

                await this.reservationService.UpdateAsync(addReservationModel);
                return this.RedirectToAction(nameof(this.Details), new { model.Id });
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await this.reservationService.DeleteByIdAsync(id);
                return this.RedirectToAction(nameof(this.Details), new { Id = id });
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                var review = this.reservationService.GetByIdWithDeleted<EditReservationInputModel>(id);
                return this.View("Edit", review);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Restore(string id)
        {
            try
            {
                await this.reservationService.RestoreAsync(id);
                return this.RedirectToAction(nameof(this.Details), new { Id = id });
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                var reservation = this.reservationService.GetById<EditReservationInputModel>(id);
                return this.View("Edit", reservation);
            }
        }
    }
}
