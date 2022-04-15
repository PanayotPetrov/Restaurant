namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Services.Models;

    public interface IUserMessageService
    {
        public IEnumerable<T> GetCategories<T>();

        public Task<string> CreateAsync(AddUserMessageModel model);
    }
}
