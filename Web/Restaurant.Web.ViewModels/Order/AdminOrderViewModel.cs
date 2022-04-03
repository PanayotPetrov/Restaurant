namespace Restaurant.Web.ViewModels.Order
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class AdminOrderViewModel : OrderViewModel, IMapFrom<Order>
    {
    }
}
