namespace Restaurant.Web.ViewModels.Home
{
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class UserMessageCategoryViewModel : IMapFrom<UserMessageCategory>
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsInSecondaryLanguage { get; set; }
    }
}
