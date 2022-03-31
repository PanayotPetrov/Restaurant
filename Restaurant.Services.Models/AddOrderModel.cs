using Restaurant.Data.Models;
using Restaurant.Services.Mapping;
using System.Collections.Generic;

namespace Restaurant.Services.Models
{
    public class AddOrderModel : IMapTo<Order>
    {
        public string FullName { get; set; }

        public string ApplicationUserId { get; set; }

        public int AddressName { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
