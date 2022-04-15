namespace Restaurant.Web.ViewModels.InputModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels.Home;

    public class UserMessageInputModel : IMapFrom<UserMessage>, IMapTo<AddUserMessageModel>
    {
        public string ApplicationUserId { get; set; }

        [Range(1, 7, ErrorMessage = "Invalid category selected.")]
        public int UserMessageCategoryId { get; set; }

        [Required(ErrorMessage = "You'll need to provide your name before we can proceed.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You'll need to provide your email before we can proceed.")]
        [EmailAddress(ErrorMessage = "Invalid email provided.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please provide a short summary.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "In this field you can explain in detail.")]
        public string Description { get; set; }

        public IEnumerable<UserMessageCategoryViewModel> Categories { get; set; }
    }
}
