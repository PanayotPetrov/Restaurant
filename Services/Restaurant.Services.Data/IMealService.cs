namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Services.Models;

    public interface IMealService
    {
        Task<int> CreateAsync(AddMealModel model, string imagePath);

        int GetMealCount();

        IEnumerable<T> GetAllMeals<T>();

        public IEnumerable<T> GetAllWithPagination<T>(int itemsPerPage, int page);

        Task<bool> DeleteByIdAsync(int mealId);

        Task UpdateAsync(EditMealModel addMealModel, string imagePath);

        bool IsMealIdValid(int mealId);

        Task<bool> RestoreAsync(int id);

        T GetByIdWithDeleted<T>(int id);
    }
}
