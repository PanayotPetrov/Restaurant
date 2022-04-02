namespace Restaurant.Web.ViewModels.Address
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;

    public class AddressViewModel : BaseAddressViewModel, IMapFrom<Address>, IMapTo<AddAddressModel>, IHaveCustomMappings
    {
        public IEnumerable<string> AddressNames { get; set; }

        [Required]
        public bool IsPrimaryAddress { get; set; }

        public IEnumerable<string> AllowedDistricts { get; set; }

        public string ApplicationUserId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Address, AddressViewModel>()
            .ForMember(a => a.AddressNames, opt => opt.MapFrom(x => x.ApplicationUser.Addresses.OrderBy(a => a.Name).Select(y => y.Name).ToList()));
        }
    }
}
