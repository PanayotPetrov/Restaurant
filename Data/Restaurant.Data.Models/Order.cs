namespace Restaurant.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    using Restaurant.Data.Common.Models;

    public class Order : BaseDeletableModel<int>
    {
        public Order()
        {
            this.Meals = new HashSet<Meal>();
        }

        public decimal TotalPrice
        {
            get => this.Meals.Sum(m => m.Price);
            set => this.TotalPrice = value;
        }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public int AddressId { get; set; }

        public virtual Address Address { get; set; }

        public virtual ICollection<Meal> Meals { get; set; }
    }
}
