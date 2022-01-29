namespace Restaurant.Data.Models
{
    using System;

    using Restaurant.Data.Common.Models;

    public class Reservation : BaseDeletableModel<int>
    {
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public DateTime ReservationDate { get; set; }

        public int NumberOfPeople { get; set; }

         // TODO: Table?
    }
}
