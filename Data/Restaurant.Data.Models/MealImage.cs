namespace Restaurant.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Data.Common.Models;

    public class MealImage : BaseModel<string>
    {
        public MealImage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int MealId { get; set; }

        public virtual Meal Meal { get; set; }

        [Required]
        public string Extension { get; set; }
    }
}
