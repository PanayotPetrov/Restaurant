using Restaurant.Data.Models;
using Restaurant.Services.Mapping;

namespace Restaurant.Services.Models
{
    public class AddReviewModel : IMapTo<Review>
    {
        public string ApplicationUserId { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }

        public string Rating { get; set; }
    }
}
