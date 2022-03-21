namespace Restaurant.Web.ViewModels.InputModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;

    public class EditMealInputModel : IMapFrom<Meal>, IMapTo<AddMealModel>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public IFormFile Image { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Categories { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Meal, EditMealInputModel>().ForMember(
                 m => m.ImageUrl,
                 opt => opt.MapFrom(x => "~/images/meals/" + x.Image.Id + '.' + x.Image.Extension))
                .ForMember(m => m.Image, opt => opt.Ignore());
        }
    }
}
