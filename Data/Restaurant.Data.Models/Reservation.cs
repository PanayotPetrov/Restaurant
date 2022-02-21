namespace Restaurant.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Restaurant.Data.Common.Models;

    public class Reservation : BaseDeletableModel<string>
    {
        public Reservation()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ReservationNumber = string.Join(string.Empty,this.Id.Take(6));
        }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        [Required]

        public string PhoneNumber { get; set; }

        public string ReservationNumber { get; set; }

        public DateTime ReservationDate { get; set; }

        public int NumberOfPeople { get; set; }

        public int TableId { get; set; }

        public virtual Table Table { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
