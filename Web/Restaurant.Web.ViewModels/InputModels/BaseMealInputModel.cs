namespace Restaurant.Web.ViewModels.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class BaseMealInputModel
    {
        [Required(ErrorMessage = "Please provide a name for this meal.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please provide a description for this meal.")]
        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The provided price is invalid.")]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Categories { get; set; }
    }
}
