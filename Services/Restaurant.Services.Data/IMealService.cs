namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Services.Models;

    public interface IMealService : IPaginationService
    {
        Task<int> CreateAsync(AddMealModel model, string imagePath);

        IEnumerable<T> GetAllMeals<T>();

        Task<bool> DeleteByIdAsync(int mealId);

        Task UpdateAsync(EditMealModel addMealModel, string imagePath);

        bool IsMealIdValid(int mealId);

        Task<bool> RestoreAsync(int id);

        T GetById<T>(int id, bool getDeleted = false);
    }
}
