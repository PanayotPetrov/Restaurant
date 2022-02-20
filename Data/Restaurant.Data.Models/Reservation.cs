namespace Restaurant.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Data.Common.Models;

    public class Reservation : BaseDeletableModel<string>
    {
        public Reservation()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string Fullname { get; set; }

        public DateTime ReservationDate { get; set; }

        public int NumberOfPeople { get; set; }

        public int TableId { get; set; }

        public virtual Table Table { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
