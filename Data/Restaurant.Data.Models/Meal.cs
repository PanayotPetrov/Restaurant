namespace Restaurant.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Data.Common.Models;

    public class Meal : BaseDeletableModel<int>
    {
        public Meal()
        {
            this.Orders = new HashSet<Order>();
            this.CartItems = new HashSet<CartItem>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual MealImage Image { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
