namespace Restaurant.Web.ViewModels.Order
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Web.ViewModels.Address;

    public class OrderViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public decimal TotalPrice { get; set; }

        public string OrderNumber { get; set; }

        public decimal ShippingPrice { get; set; }

        public string ApplicationUserId { get; set; }

        public AddressViewModel Address { get; set; }

        public bool IsComplete { get; set; }

        public DateTime DeliveryTime { get; set; }

        public virtual IEnumerable<MealOrderViewModel> Meals { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, OrderViewModel>().ForMember(o => o.ShippingPrice, opt => opt.MapFrom(c => c.ApplicationUser.Cart.ShippingPrice));
        }
    }
}
