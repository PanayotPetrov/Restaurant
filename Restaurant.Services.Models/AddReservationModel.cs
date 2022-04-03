using AutoMapper;
using Restaurant.Data.Models;
using Restaurant.Services.Mapping;
using System;

namespace Restaurant.Services.Models
{
    public class AddReservationModel : IMapTo<Reservation>, IHaveCustomMappings
    {
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string SpecialRequest { get; set; }

        public DateTime ReservationDate { get; set; }

        public int ReservationTime { get; set; }

        public int NumberOfPeople { get; set; }

        public string ApplicationUserId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<AddReservationModel, Reservation>().ForMember(r => r.ReservationDate, opt => opt.MapFrom(x => x.ReservationDate.ToUniversalTime()));
        }
    }
}
