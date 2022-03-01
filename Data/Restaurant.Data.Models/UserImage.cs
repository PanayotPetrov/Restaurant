namespace Restaurant.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Data.Common.Models;

    public class UserImage : BaseModel<string>
    {
        public UserImage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string Extension { get; set; }
    }
}
