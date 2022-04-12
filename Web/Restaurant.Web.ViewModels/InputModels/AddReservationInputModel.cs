namespace Restaurant.Web.ViewModels.InputModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.Infrastructure.ValidationAttributes;

    public class AddReservationInputModel : IMapTo<AddReservationModel>, IHaveCustomMappings
    {
        [Required(ErrorMessage = "We need your first name to book a table.")]
        [Display(Name = "Full name")]

        public string FullName { get; set; }

        [Required(ErrorMessage = "We need your phone number in order to book a table.")]
        [Phone(ErrorMessage = "Invalid phone number provided. Please try again.")]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "We need your email in order to book a table.")]
        [EmailAddress(ErrorMessage = "Invalid email provided. Please try again.")]
        public string Email { get; set; }

        [MaxLength(200, ErrorMessage = "The maximum number of characters is 200.")]
        public string SpecialRequest { get; set; }

        [CurrentDateValidation]
        [Required(ErrorMessage = "We can't book a table unless you select a date.")]
        public DateTime ReservationDate { get; set; }

        [Range(18, 22, ErrorMessage = "Your reservation must be between 18:00 and 22:00")]
        public int ReservationTime { get; set; }

        [Range(2, 6, ErrorMessage = "Our tables currently fit between 2 and 6 people")]
        public int NumberOfPeople { get; set; }

        public string ApplicationUserId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<AddReservationInputModel, AddReservationModel>().ForMember(r => r.ReservationDate, opt => opt.MapFrom(r => r.ReservationDate.AddHours(r.ReservationTime)));
        }
    }
}
