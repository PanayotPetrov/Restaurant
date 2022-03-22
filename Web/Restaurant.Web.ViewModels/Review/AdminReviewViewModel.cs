namespace Restaurant.Web.ViewModels.Review
{
    using System;

    using AutoMapper;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class AdminReviewViewModel : IMapFrom<Review>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

        public string FullName { get; set; }

        public string Description { get; set; }

        public int Rating { get; set; }

        public string Summary { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, AdminReviewViewModel>()
                .ForMember(r => r.FullName, opt => opt.MapFrom(x => $"{x.ApplicationUser.FirstName} {x.ApplicationUser.LastName}"))
                .ForMember(r => r.ImageUrl, opt => opt.MapFrom(x => x.ApplicationUser.UserImage != null
                ? "~/images/users/" + x.ApplicationUser.UserImage.Id + '.' + x.ApplicationUser.UserImage.Extension
                : "~/images/default.png"));
        }
    }
}
