namespace Restaurant.Data.Models
{
    using System.Collections.Generic;

    using Restaurant.Data.Common.Models;

    public class CartItem : BaseModel<int>
    {
        public int MealId { get; set; }

        public virtual Meal Meal { get; set; }

        public int Quantity { get; set; }

        public decimal ItemTotalPrice { get; set; }

        public int CartId { get; set; }

        public virtual Cart Cart { get; set; }
    }
}
