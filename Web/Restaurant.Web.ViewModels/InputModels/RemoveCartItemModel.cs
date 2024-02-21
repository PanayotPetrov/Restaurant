namespace Restaurant.Web.ViewModels.InputModels
{
    using AutoMapper;
    using Restaurant.Common.Resources;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.Infrastructure.ValidationAttributes;

    public class RemoveCartItemModel : IMapTo<CartItemModel>, IHaveCustomMappings
    {
        [MealIdValidation(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_INVALID_MEAL_ID")]
        public int MealId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<RemoveCartItemModel, CartItemModel>().ForMember(m => m.Quantity, opt => opt.Ignore());
        }
    }
}
