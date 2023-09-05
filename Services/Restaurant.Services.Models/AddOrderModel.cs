using Restaurant.Data.Models;
using Restaurant.Services.Mapping;

namespace Restaurant.Services.Models
{
    public class AddOrderModel : IMapTo<Order>
    {
        public int AddressId { get; set; }

        public string ApplicationUserId { get; set; }

        public int CartId { get; set; }
    }
}
