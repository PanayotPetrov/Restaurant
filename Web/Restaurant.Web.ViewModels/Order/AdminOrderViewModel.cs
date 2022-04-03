namespace Restaurant.Web.ViewModels.Order
{
    using System;

    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class AdminOrderViewModel : OrderViewModel, IMapFrom<Order>
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime DeletedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
