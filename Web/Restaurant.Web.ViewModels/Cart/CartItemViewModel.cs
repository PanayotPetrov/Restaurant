namespace Restaurant.Web.ViewModels.Cart
{
    using AutoMapper;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Web.ViewModels.Meal;

    public class CartItemViewModel : IMapFrom<CartItem>, IHaveCustomMappings
    {
        public MealViewModel Meal { get; set; }

        public int Quantity { get; set; }

        public string ItemTotalPrice { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<CartItem, CartItemViewModel>()
                .ForMember(ci => ci.ItemTotalPrice, opt => opt.MapFrom(x => x.ItemTotalPrice.ToString("0.00")));
        }
    }
}
