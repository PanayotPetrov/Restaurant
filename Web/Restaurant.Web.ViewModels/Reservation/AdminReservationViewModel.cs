namespace Restaurant.Web.ViewModels.Reservation
{
    using System;

    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class AdminReservationViewModel : ReservationViewModel, IMapFrom<Reservation>
    {
        public string Id { get; set; }

        public string ApplicationUserId { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
