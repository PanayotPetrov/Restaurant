namespace Restaurant.Web.ViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.Infrastructure.ValidationAttributes;

    public class CreateOrderInputModel : IMapTo<AddOrderModel>, IHaveCustomMappings
    {
        [CartIdValidation]
        public int CartId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid address selected")]
        public int AddressId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "You need to add at least 1 product to place an order!")]
        public int CartItemsCount { get; set; }

        [Required(ErrorMessage = "We need your number in order to proceed. Please update it in your account.")]
        [Phone(ErrorMessage = "Invalid phone number provided.")]

        public string PhoneNumber { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<CreateOrderInputModel, AddOrderModel>().ForMember(x => x.ApplicationUserId, opt => opt.Ignore());
        }
    }
}
