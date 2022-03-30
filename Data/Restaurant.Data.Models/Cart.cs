namespace Restaurant.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Restaurant.Data.Common.Models;

    public class Cart : BaseDeletableModel<int>
    {
        public Cart()
        {
            this.CartItems = new HashSet<CartItem>();
        }

        [Required]
        public string ApplicationUserId { get; set; }

        public decimal ShippingPrice { get; set; }

        public decimal SubTotal { get; set; }

        public decimal TotalPrice
        {
            get => this.ShippingPrice + this.SubTotal;
            set { }
        }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
