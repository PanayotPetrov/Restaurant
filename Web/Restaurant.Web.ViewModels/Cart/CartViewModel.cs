namespace Restaurant.Web.ViewModels.Cart
{
    using System.Collections.Generic;

    using AutoMapper;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class CartViewModel : IMapFrom<Cart>, IHaveCustomMappings
    {
        public IEnumerable<CartItemViewModel> CartItems { get; set; }

        public string TotalPrice { get; set; }

        public string ShippingPrice { get; set; }

        public string SubTotal { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Cart, CartViewModel>()
                .ForMember(m => m.TotalPrice, opt => opt.MapFrom(c => c.TotalPrice.ToString("0.00")))
                .ForMember(m => m.ShippingPrice, opt => opt.MapFrom(c => c.ShippingPrice.ToString("0.00")))
                .ForMember(m => m.SubTotal, opt => opt.MapFrom(c => c.SubTotal.ToString("0.00")));
        }
    }
}
