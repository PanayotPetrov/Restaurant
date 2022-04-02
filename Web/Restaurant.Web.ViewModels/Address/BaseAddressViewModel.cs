namespace Restaurant.Web.ViewModels.Address
{
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Web.Infrastructure.ValidationAttributes;

    public class BaseAddressViewModel : IMapFrom<Address>
    {
        [Required]
        [ValidateUniqueAddressName]
        public string Name { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string AddressLineTwo { get; set; }

        [Required]
        [AllowedDistrictsValidationAttribute]
        public string District { get; set; }

        [Required]
        [AllowedCityValidationAttribute]
        public string City { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        [AllowedCountryValidationAttribute]
        public string Country { get; set; }
    }
}
