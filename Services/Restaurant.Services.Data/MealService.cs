namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;

    public class MealService : IMealService
    {
        private readonly IDeletableEntityRepository<Meal> mealRepository;

        public MealService(IDeletableEntityRepository<Meal> mealRepository)
        {
            this.mealRepository = mealRepository;
        }

        public IEnumerable<T> GetAllMeals<T>()
        {
            return this.mealRepository.All().Include(i => i.MealType).To<T>().ToList();
        }

        public int GetMealCount()
        {
            return this.mealRepository.AllAsNoTracking().Count();
        }
    }
}
