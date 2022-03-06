namespace Restaurant.Services.Models
{
    public class AddReviewModel
    {
        public string ApplicationUserId { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }

        public string Rating { get; set; }
    }
}
