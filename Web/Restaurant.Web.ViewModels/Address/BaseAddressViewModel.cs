namespace Restaurant.Web.ViewModels.Address
{
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Web.Infrastructure.ValidationAttributes;

    public class BaseAddressViewModel : IMapFrom<Address>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Your address name can't be empty")]
        [ValidateUniqueAddressName]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please provide a street.")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Please provide some more information - flat, floor number, etc.")]
        public string AddressLineTwo { get; set; }

        [Required]
        [AllowedDistrictsValidationAttribute]
        public string District { get; set; }

        [Required]
        [AllowedCityValidationAttribute]
        public string City { get; set; }

        [Required(ErrorMessage = "Please provide your postcode/zip.")]
        public string PostCode { get; set; }

        [Required]
        [AllowedCountryValidationAttribute]
        public string Country { get; set; }
    }
}
