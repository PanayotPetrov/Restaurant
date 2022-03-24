namespace Restaurant.Web.ViewModels.Reservation
{
    using System.Collections.Generic;

    public class AdminReservationListViewModel : PagingViewModel
    {
        public IEnumerable<AdminReservationViewModel> Reservations { get; set; }
    }
}
