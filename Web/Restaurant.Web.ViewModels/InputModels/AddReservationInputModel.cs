namespace Restaurant.Web.ViewModels.InputModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Web.Infrastructure.ValidationAttributes;

    public class AddReservationInputModel
    {
        [Required(ErrorMessage = "We need your first name to book a table.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "We need your last name to book a table.")]
        public string LastName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [MaxLength(200, ErrorMessage = "The maximum number of characters is 200.")]
        public string SpecialRequest { get; set; }

        [CurrentDateValidation]
        [Required(ErrorMessage = "We can't book a table unless you select a date.")]
        public DateTime ReservationDate { get; set; }

        [Range(18, 22, ErrorMessage = "Your reservation must be between 18:00 and 22:00")]
        public int ReservationTime { get; set; }

        [Range(2, 6)]
        public int NumberOfPeople { get; set; }

        public string UserId { get; set; }
    }
}
