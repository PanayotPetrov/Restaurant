namespace Restaurant.Data.Models
{
    public class MealOrder
    {
        public int MealId { get; set; }

        public virtual Meal Meal { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public int MealQuantity { get; set; }
    }
}
