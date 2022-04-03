namespace Restaurant.Web.ViewModels.Order
{
    using System.Collections.Generic;

    public class AdminOrderListViewModel : PagingViewModel
    {
        public IEnumerable<AdminOrderViewModel> Orders { get; set; }
    }
}
