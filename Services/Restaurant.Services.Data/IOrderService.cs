namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Services.Models;

    public interface IOrderService
    {
        Task<string> CreateAsync(AddOrderModel model);

        IEnumerable<T> GetAllByUserId<T>(string userId);

        T GetByOrderNumber<T>(string orderNumber);
    }
}
