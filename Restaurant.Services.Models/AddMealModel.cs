namespace Restaurant.Services.Models
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class AddMealModel : IMapTo<Meal>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public IFormFile Image { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<AddMealModel, Meal>()
                .ForMember(m => m.Image, opt => opt.Ignore());
        }
    }
}
