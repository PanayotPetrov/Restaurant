namespace Restaurant.Web.ViewModels.Order
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class AllOrdersViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public IEnumerable<string> OrdersByUser { get; set; }

        public OrderViewModel Order { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, AllOrdersViewModel>()
                .ForMember(o => o.OrdersByUser, opt => opt.MapFrom(x => x.ApplicationUser.Orders.OrderBy(o => o.IsComplete).Select(y => y.OrderNumber)))
                .ForMember(o => o.Order, opt => opt.MapFrom(x => x));
        }
    }
}
