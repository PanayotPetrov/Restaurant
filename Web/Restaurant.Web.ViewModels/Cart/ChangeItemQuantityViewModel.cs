namespace Restaurant.Web.ViewModels.Cart
{
    using AutoMapper;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class ChangeItemQuantityViewModel : IMapFrom<CartItem>, IHaveCustomMappings
    {
        public string ItemTotalPrice { get; set; }

        public int Quantity { get; set; }

        public string CartSubTotal { get; set; }

        public string CartTotalPrice { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<CartItem, ChangeItemQuantityViewModel>()
                .ForMember(x => x.CartTotalPrice, opt => opt.MapFrom(ci => ci.Cart.TotalPrice.ToString("0.00")))
                .ForMember(x => x.CartSubTotal, opt => opt.MapFrom(ci => ci.Cart.SubTotal.ToString("0.00")));
        }
    }
}
