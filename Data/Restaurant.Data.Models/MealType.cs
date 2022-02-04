namespace Restaurant.Data.Models
{
    using Restaurant.Data.Common.Models;

    public class MealType : BaseDeletableModel<int>
    {
        public string Name { get; set; }
    }
}
