namespace Restaurant.Web.ViewModels.Reservation
{
    using System;

    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class ReservationViewModel : IMapFrom<Reservation>
    {
        public string ReservationNumber { get; set; }

        public string Fullname { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime ReservationDate { get; set; }

        public int NumberOfPeople { get; set; }

        public int TableId { get; set; }

        public string Email { get; set; }
    }
}
