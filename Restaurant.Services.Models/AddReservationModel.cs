using System;

namespace Restaurant.Services.Models
{
    public class AddReservationModel
    {
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string SpecialRequest { get; set; }

        public DateTime ReservationDate { get; set; }

        public int ReservationTime { get; set; }

        public int NumberOfPeople { get; set; }

        public string UserId { get; set; }
    }
}
