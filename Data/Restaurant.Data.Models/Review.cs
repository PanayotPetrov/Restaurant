namespace Restaurant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Data.Common.Models;

    public class Review : BaseDeletableModel<int>
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
