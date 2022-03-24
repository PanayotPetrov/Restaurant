namespace Restaurant.Services.Models
{
    using AutoMapper;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class EditMealModel : AddMealModel, IHaveCustomMappings
    {
        public int Id { get; set; }

        public override void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<EditMealModel, Meal>()
                .ForMember(m => m.Image, opt => opt.Ignore());
        }
    }
}
