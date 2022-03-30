namespace Restaurant.Web.ViewModels.Cart
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ChangeItemQuantityViewModel
    {
        public string ItemTotalPrice { get; set; }

        public string ItemQuantity { get; set; }

        public string CartSubTotal { get; set; }

        public string CartTotalPrice { get; set; }
    }
}
