namespace Restaurant.Web.ViewModels.InputModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Common.Resources;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels.Home;

    public class UserMessageInputModel : IMapFrom<UserMessage>, IMapTo<AddUserMessageModel>
    {
        public string ApplicationUserId { get; set; }

        public int UserMessageCategoryId { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_NAME")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_EMAIL")]
        [EmailAddress(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_INVALID_EMAIL")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_SUMMARY")]
        public string Subject { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_DESCRIPTION")]
        public string Description { get; set; }

        public IEnumerable<UserMessageCategoryViewModel> Categories { get; set; }
    }
}
