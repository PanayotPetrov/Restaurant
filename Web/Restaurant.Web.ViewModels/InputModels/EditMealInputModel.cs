namespace Restaurant.Web.ViewModels.InputModels
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;

    public class EditMealInputModel : BaseMealInputModel, IMapFrom<Meal>, IMapTo<EditMealModel>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public IFormFile Image { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Meal, EditMealInputModel>().ForMember(
                 m => m.ImageUrl,
                 opt => opt.MapFrom(x => "~/images/meals/" + x.Image.Id + '.' + x.Image.Extension))
                .ForMember(m => m.Image, opt => opt.Ignore());
        }
    }
}
