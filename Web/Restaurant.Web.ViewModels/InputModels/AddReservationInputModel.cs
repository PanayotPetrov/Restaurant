namespace Restaurant.Web.ViewModels.InputModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddReservationInputModel
    {
        [Required(ErrorMessage = "We need your fullname to book a table.")]
        public string FullName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [MaxLength(200, ErrorMessage = "The maximum number of characters is 200.")]
        public string SpecialRequest { get; set; }

        // TO DO: Add validation for current date
        [Required(ErrorMessage = "We can't book a table unless you select a date.")]
        public DateTime DateAndTime { get; set; }

        public int NumberOfPeople { get; set; }
    }
}
