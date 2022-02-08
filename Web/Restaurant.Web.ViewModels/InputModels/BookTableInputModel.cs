namespace Restaurant.Web.ViewModels.InputModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class BookTableInputModel
    {
        [Required]
        public string FullName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        public string SpecialRequest { get; set; }

        public DateTime DateAndTime { get; set; }

        [Range(1,3)]
        public int NumberOfPeople { get; set; }
    }
}
