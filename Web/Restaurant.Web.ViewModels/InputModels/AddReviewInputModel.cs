namespace Restaurant.Web.ViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;

    public class AddReviewInputModel : IMapTo<AddReviewModel>
    {
        public string ApplicationUserId { get; set; }

        [Required]
        [MaxLength(400)]
        public string Description { get; set; }

        [Required]
        [MaxLength(36)]
        public string Summary { get; set; }

        [Required(ErrorMessage = "Please select a rating!")]
        public string Rating { get; set; }
    }
}
