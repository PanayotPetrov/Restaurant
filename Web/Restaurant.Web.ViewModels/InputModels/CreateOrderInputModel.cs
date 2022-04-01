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

        [AddressNameValidation]
        public string AddressName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage ="You need to add at least 1 product to place an order!")]
        public int CartItemsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<CreateOrderInputModel, AddOrderModel>().ForMember(x => x.ApplicationUserId, opt => opt.Ignore());
        }
    }
}
