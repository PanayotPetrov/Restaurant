namespace Restaurant.Data.Models
{
    using System.Collections.Generic;

    using Restaurant.Data.Common.Models;

    public class CartItem : BaseModel<int>
    {
        public CartItem()
        {
            this.Carts = new HashSet<Cart>();
        }

        public int MealId { get; set; }

        public virtual Meal Meal { get; set; }

        public int Quantity { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
    }
}
