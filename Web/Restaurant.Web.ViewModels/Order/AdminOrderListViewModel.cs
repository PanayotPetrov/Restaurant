namespace Restaurant.Web.ViewModels.Order
{
    using System.Collections.Generic;

    public class AdminOrderListViewModel : PagingViewModel
    {
        public IEnumerable<OrderViewModel> Orders { get; set; }
    }
}
