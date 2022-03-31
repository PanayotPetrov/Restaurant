namespace Restaurant.Web.ViewModels.Checkout
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Web.ViewModels.Cart;

    public class CheckoutViewModel : CartViewModel, IMapFrom<Cart>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public IEnumerable<string> Addresses { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public override void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Cart, CheckoutViewModel>()
                .ForMember(m => m.TotalPrice, opt => opt.MapFrom(c => c.TotalPrice.ToString("0.00")))
                .ForMember(m => m.ShippingPrice, opt => opt.MapFrom(c => c.ShippingPrice.ToString("0.00")))
                .ForMember(m => m.SubTotal, opt => opt.MapFrom(c => c.SubTotal.ToString("0.00")))
                .ForMember(m => m.Addresses, opt => opt.MapFrom(c => c.ApplicationUser.Addresses.OrderBy(a => a.IsPrimaryAddress).Select(a => a.Name)))
                .ForMember(m => m.FirstName, opt => opt.MapFrom(c => c.ApplicationUser.FirstName))
                .ForMember(m => m.LastName, opt => opt.MapFrom(c => c.ApplicationUser.LastName))
                .ForMember(m => m.Email, opt => opt.MapFrom(c => c.ApplicationUser.Email))
                .ForMember(m => m.PhoneNumber, opt => opt.MapFrom(c => c.ApplicationUser.PhoneNumber));
        }
    }
}
