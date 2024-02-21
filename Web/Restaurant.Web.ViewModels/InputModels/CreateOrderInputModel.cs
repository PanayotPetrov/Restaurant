namespace Restaurant.Web.ViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Restaurant.Common.Resources;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.Infrastructure.ValidationAttributes;

    public class CreateOrderInputModel : IMapTo<AddOrderModel>, IHaveCustomMappings
    {
        [CartIdValidation(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_INVALID_CART_ID")]
        public int CartId { get; set; }

        [Range(1, 100, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_RANGE_ADDRESS")]
        public int AddressId { get; set; }

        [Range(1, 100, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_RANGE_CART_ITEM_QUANTITY")]
        public int CartItemsCount { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_PHONE_NUMBER")]
        [Phone(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_INVALID_PHONE_NUMBER")]

        public string PhoneNumber { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<CreateOrderInputModel, AddOrderModel>().ForMember(x => x.ApplicationUserId, opt => opt.Ignore());
        }
    }
}
