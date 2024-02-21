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

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_FIELD")]
        [ValidateUniqueAddressName]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_FIELD")]
        public string Street { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_ADDRESS_LINE_TWO")]
        public string AddressLineTwo { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_FIELD")]
        [AllowedDistrictsValidation(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_INVALID_DISTRICT")]
        public string District { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_FIELD")]
        [AllowedCityValidation(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_INVALID_CITY")]
        public string City { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_FIELD")]
        public string PostCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_FIELD")]
        [AllowedCountryValidation(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_INVALID_COUNTRY")]
        public string Country { get; set; }
    }
}
