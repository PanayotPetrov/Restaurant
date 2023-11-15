namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Restaurant.Data.Models;
    using Restaurant.Data.Repositories;
    using Restaurant.Services.Mapping;

    public class CategoryService : ICategoryService
    {
        private readonly IRestaurantDeletableEntityRepositoryDecorator<Category> categoriesRepository;

        public CategoryService(
            IRestaurantDeletableEntityRepositoryDecorator<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.categoriesRepository.AllAsNoTracking().To<T>().ToList();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.categoriesRepository.AllAsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                })
                .OrderBy(x => x.Name)
                .ToList().Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }
    }
}
