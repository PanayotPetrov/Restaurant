namespace Restaurant.Web.ViewModels.InputModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Restaurant.Common.Resources;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.Infrastructure.ValidationAttributes;

    public class AddReservationInputModel : IMapTo<AddReservationModel>, IHaveCustomMappings
    {
        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_NAME")]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_PHONE_NUMBER")]
        [Phone(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_INVALID_PHONE_NUMBER")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_EMAIL")]
        [EmailAddress(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_INVALID_EMAIL")]
        public string Email { get; set; }

        [MaxLength(200, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_MAX_CHARS")]
        public string SpecialRequest { get; set; }

        [CurrentDateValidation]
        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_DATE")]
        public DateTime ReservationDate { get; set; }

        [Range(18, 22, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_RANGE_RESERVATION_TIME")]
        public int ReservationTime { get; set; }

        [Range(2, 6, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_RANGE_NUMBER_OF_PEOPLE")]
        public int NumberOfPeople { get; set; }

        public string ApplicationUserId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<AddReservationInputModel, AddReservationModel>().ForMember(r => r.ReservationDate, opt => opt.MapFrom(r => r.ReservationDate.AddHours(r.ReservationTime)));
        }
    }
}
