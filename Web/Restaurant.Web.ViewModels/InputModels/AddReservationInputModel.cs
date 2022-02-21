namespace Restaurant.Web.ViewModels.InputModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Web.Infrastructure.ValidationAttributes;

    public class AddReservationInputModel
    {
        [Required(ErrorMessage = "We need your first name to book a table.")]
        [Display(Name = "Full name")]

        public string FullName { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
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
