namespace Restaurant.Web.ViewModels.Order
{
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class MealOrderViewModel : IMapFrom<MealOrder>
    {
        public int MealId { get; set; }

        public int MealQuantity { get; set; }

        public string MealName { get; set; }

        public decimal MealPrice { get; set; }
    }
}
