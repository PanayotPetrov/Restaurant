namespace Restaurant.Data.Models
{
    using System.Collections.Generic;

    using Restaurant.Data.Common.Models;

    public class Address : BaseDeletableModel<int>
    {
        public Address()
        {
            this.Orders = new HashSet<Order>();
        }

        public string Street { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        public string District { get; set; }

        public string Country { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
