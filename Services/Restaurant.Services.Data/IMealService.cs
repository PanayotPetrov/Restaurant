namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Services.Models;

    public interface IMealService
    {
        Task CreateAsync(AddMealModel model, string imagePath);

        int GetMealCount();

        IEnumerable<T> GetAllMeals<T>();
    }
}
