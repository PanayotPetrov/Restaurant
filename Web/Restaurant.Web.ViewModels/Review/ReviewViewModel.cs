namespace Restaurant.Web.ViewModels.Review
{
    using AutoMapper;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class ReviewViewModel : IMapFrom<Review>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Description { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, ReviewViewModel>().ForMember(
                r => r.ApplicationUserName,
                opt => opt.MapFrom(x => x.ApplicationUser.UserName));
        }
    }
}
