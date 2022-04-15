using Restaurant.Data.Models;
using Restaurant.Services.Mapping;

namespace Restaurant.Services.Models
{
    public class AddUserMessageModel : IMapTo<UserMessage>
    {
        public string ApplicationUserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }
    }
}
