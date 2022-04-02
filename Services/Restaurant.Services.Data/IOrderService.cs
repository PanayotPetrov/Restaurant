namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Services.Models;

    public interface IOrderService
    {
        Task<string> CreateAsync(AddOrderModel model);

        T GetByUserIdAndOrderNumber<T>(string userId, string orderNumber);

        IEnumerable<string> GetAllOrderNumbersByUserId(string userId);
    }
}
