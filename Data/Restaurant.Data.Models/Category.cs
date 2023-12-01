namespace Restaurant.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Restaurant.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.Meals = new HashSet<Meal>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Adjective { get; set; }

        [Required]
        public string SecondaryName { get; set; }

        [Required]
        public string SecondaryAdjective { get; set; }

        [Required]
        public string FontAwesomeIcon { get; set; }

        public virtual ICollection<Meal> Meals { get; set; }
    }
}
