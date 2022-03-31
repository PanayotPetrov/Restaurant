﻿namespace Restaurant.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;
    using Restaurant.Data.Common.Models;

    [Index(nameof(OrderNumber), IsUnique = false)]

    public class Order : BaseDeletableModel<int>
    {
        public Order()
        {
            this.Meals = new HashSet<Meal>();
        }

        public decimal TotalPrice { get; set; }

        [Required]
        public string FullName { get; set; }

        public string OrderNumber { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Address")]
        public string AddressName { get; set; }

        public virtual Address Address { get; set; }

        public virtual ICollection<Meal> Meals { get; set; }
    }
}
