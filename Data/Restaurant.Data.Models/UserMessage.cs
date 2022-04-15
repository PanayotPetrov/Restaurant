namespace Restaurant.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Data.Common.Models;

    public class UserMessage : BaseDeletableModel<string>
    {
        public UserMessage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Description { get; set; }

        public UserMessageCategory Category { get; set; }
    }
}
