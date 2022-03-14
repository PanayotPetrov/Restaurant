namespace Restaurant.Web.ViewModels.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class AddMealInputModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Categories { get; set; }
    }
}
