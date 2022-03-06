namespace Restaurant.Web.ViewModels.Meal
{
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

        public CategoryViewModel Category { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Meal, MealViewModel>().ForMember(
                m => m.ImageUrl,
                opt => opt.MapFrom(x => "~/images/meals/" + x.Image.Id + '.' + x.Image.Extension));
        }
    }
}
