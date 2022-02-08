namespace Restaurant.Services.Data
{
    using System.Collections.Generic;

    public interface IMealService
    {
        int GetMealCount();

        IEnumerable<T> GetAllMeals<T>();
    }
}
