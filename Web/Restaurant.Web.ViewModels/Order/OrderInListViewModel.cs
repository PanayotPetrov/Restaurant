namespace Restaurant.Web.ViewModels.Order
{
    using System.Collections.Generic;

    public class OrderInListViewModel
    {
        public IEnumerable<OrderViewModel> Orders { get; set; }
    }
}
