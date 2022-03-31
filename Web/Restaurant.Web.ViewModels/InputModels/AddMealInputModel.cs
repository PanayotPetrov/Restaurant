namespace Restaurant.Web.ViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;

    public class AddMealInputModel : BaseMealInputModel, IMapTo<AddMealModel>
    {
        [Required]
        public virtual IFormFile Image { get; set; }
    }
}
