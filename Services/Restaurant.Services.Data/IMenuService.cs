namespace Restaurant.Services.Data
{
    using System.Collections.Generic;

    public interface IMenuService
    {
        int GetMealCount();

        IEnumerable<T> GetAllMeals<T>();
    }
}
