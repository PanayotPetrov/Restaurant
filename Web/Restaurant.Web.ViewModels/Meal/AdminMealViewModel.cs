namespace Restaurant.Web.ViewModels.Meal
{
    using System;

    using AutoMapper;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Web.ViewModels.Category;

    public class AdminMealViewModel : MealViewModel, IMapFrom<Meal>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public override void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Meal, AdminMealViewModel>().ForMember(
                m => m.ImageUrl,
                opt => opt.MapFrom(x => "~/images/meals/" + x.Image.Id + '.' + x.Image.Extension));
        }
    }
}
