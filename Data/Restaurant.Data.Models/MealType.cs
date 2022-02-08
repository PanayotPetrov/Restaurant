namespace Restaurant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Data.Common.Models;

    public class MealType : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }
    }
}
