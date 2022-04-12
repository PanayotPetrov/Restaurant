namespace Restaurant.Web.Areas.Administration.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.Infrastructure.Filters;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
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

            var reservations = this.reservationService.GetAllWithoutPassedDates<AdminReservationViewModel>(ItemsPerPage, id);

            var model = new AdminReservationListViewModel
            {
                Reservations = reservations,
                ItemsPerPage = ItemsPerPage,
                ItemCount = this.reservationService.GetCount(),
                PageNumber = id,
            };

            if (id > model.PagesCount && reservations.Count() != 0)
            {
                return this.NotFound();
            }

            this.TempData["RouteId"] = id;

            return this.View(model);
        }

        [GetModelErrorsFromTempDataActionFilter]
        public IActionResult Details([ReservationIdValidation][Required] string id)
        {
            if (!this.ModelState.IsValid && !this.ModelState.Keys.Contains("Tempdata error"))
            {
                return this.NotFound();
            }

            var model = this.reservationService.GetByIdWithDeleted<AdminReservationViewModel>(id);

            return this.View(model);
        }

        public IActionResult Edit([ReservationIdValidation][Required] string id)
        {
            if (!this.ModelState.IsValid)
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
        [AddModelErrorsToTempDataActionFilter]
        public async Task<IActionResult> Delete([ReservationIdValidation] string id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            var result = await this.reservationService.DeleteByIdAsync(id);

            if (!result)
            {
                this.ModelState.AddModelError("Tempdata error", "This reservation has already been deleted!");
            }

            return this.RedirectToAction(nameof(this.Details), new { id });
        }

        [HttpPost]
        [AddModelErrorsToTempDataActionFilter]
        public async Task<IActionResult> Restore([ReservationIdValidation] string id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            var result = await this.reservationService.RestoreAsync(id);

            if (!result)
            {
                this.ModelState.AddModelError("Tempdata error", "Cannot restore a reservation which has not been deleted!");
            }

            return this.RedirectToAction(nameof(this.Details), new { id });
        }
    }
}
