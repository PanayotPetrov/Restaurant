namespace Restaurant.Web.ViewModels.Address
{
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Common.Resources;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Web.Infrastructure.ValidationAttributes;

    public class BaseAddressViewModel : IMapFrom<Address>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Your address name can't be empty")]
        [ValidateUniqueAddressName]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_FIELD")]
        public string Street { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_ADDRESS_LINE_TWO")]
        public string AddressLineTwo { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_FIELD")]
        [AllowedDistrictsValidationAttribute]
        public string District { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_FIELD")]
        [AllowedCityValidationAttribute]
        public string City { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_FIELD")]
        public string PostCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_FIELD")]
        [AllowedCountryValidationAttribute]
        public string Country { get; set; }
    }
}
