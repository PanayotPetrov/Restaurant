namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;

    public class CategoryService : ICategoryService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public CategoryService(
            IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
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
