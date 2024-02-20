namespace Restaurant.Web.ViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Common.Resources;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;

    public class AddReviewInputModel : IMapTo<AddReviewModel>
    {
        public string ApplicationUserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_DESCRIPTION")]
        [MaxLength(400, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_MAX_CHARS")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_SUMMARY")]
        [MaxLength(36, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_MAX_CHARS")]
        public string Summary { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_RATING")]
        public string Rating { get; set; }
    }
}
