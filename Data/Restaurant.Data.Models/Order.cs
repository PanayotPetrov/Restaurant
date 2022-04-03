﻿namespace Restaurant.Data.Models
{
    using System;
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
            this.Meals = new HashSet<MealOrder>();
        }

        public decimal TotalPrice { get; set; }

        public string OrderNumber { get; set; }

        public bool IsComplete { get; set; }

        public DateTime DeliveryTime { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Address")]
        public int AddressId { get; set; }

        public string AddressName { get; set; }

        public virtual Address Address { get; set; }

        public virtual ICollection<MealOrder> Meals { get; set; }
    }
}
