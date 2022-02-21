namespace Restaurant.Web.ViewModels.Reservation
{
    using System.Collections.Generic;

    public class ReservationListViewModel
    {
        public IEnumerable<ReservationViewModel> Reservations { get; set; }
    }
}
