namespace Restaurant.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;
    using Restaurant.Data.Common.Models;

    [Index(nameof(Name), IsUnique = false)]
    public class Address : BaseDeletableModel<int>
    {
        public Address()
        {
            this.Orders = new HashSet<Order>();
        }

        public string Name { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string AddressLineTwo { get; set; }

        [Required]
        public string District { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public bool IsPrimaryAddress { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
