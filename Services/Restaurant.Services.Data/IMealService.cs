namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Web.ViewModels.InputModels;

    public interface IMealService
    {
        Task CreateAsync(AddMealInputModel model, string imagePath);

        int GetMealCount();

        IEnumerable<T> GetAllMeals<T>();
    }
}
