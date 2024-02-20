namespace Restaurant.Web.ViewModels.Home
{
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Common.Resources;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class UserMessageCategoryViewModel : IMapFrom<UserMessageCategory>
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "VALIDATION_REQUIRED_NAME")]
        public string Name { get; set; }

        public bool IsInSecondaryLanguage { get; set; }
    }
}
