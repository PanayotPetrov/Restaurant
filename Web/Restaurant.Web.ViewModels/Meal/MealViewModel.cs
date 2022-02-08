namespace Restaurant.Web.ViewModels.Meal
{
    using System.Collections.Generic;

    using AutoMapper;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Web.ViewModels.Category;

    public class MealViewModel : IMapFrom<Meal>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string MealType { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Meal, MealViewModel>().ForMember(
                m => m.MealType,
                opt => opt.MapFrom(x => x.MealType.Name));
        }
    }
}
