namespace Restaurant.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Data.Common.Models;

    public class Address : BaseDeletableModel<int>
    {
        public Address()
        {
            this.Orders = new HashSet<Order>();
        }

        [Required]

        public string Street { get; set; }

        [Required]

        public string District { get; set; }

        [Required]

        public string City { get; set; }

        [Required]

        public string PostCode { get; set; }

        [Required]

        public string Country { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
