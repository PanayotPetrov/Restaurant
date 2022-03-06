namespace Restaurant.Web.ViewModels.Address
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class AddressViewModel : IMapFrom<Address>, IHaveCustomMappings
    {
        public IEnumerable<string> AddressNames { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string District { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public bool IsPrimaryAddress { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Address, AddressViewModel>()
            .ForMember(a => a.AddressNames, opt => opt.MapFrom(x => x.ApplicationUser.Addresses.OrderBy(a => a.Name).Select(y => y.Name).ToList()));
        }
    }
}
