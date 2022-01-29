namespace Restaurant.Data.Models
{
    using Restaurant.Data.Common.Models;

    public class Review : BaseDeletableModel<int>
    {
        public string Description { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
