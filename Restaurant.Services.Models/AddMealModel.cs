namespace Restaurant.Services.Models
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;

    public class AddMealModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public IFormFile Image { get; set; }
    }
}
