namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Services.Models;

    public interface IOrderService : IPaginationService
    {
        Task<string> CreateAsync(AddOrderModel model);

        T GetByUserIdAndOrderNumber<T>(string userId, string orderNumber);

        IEnumerable<string> GetAllOrderNumbersByUserId(string userId);

        T GetByOrderNumber<T>(string orderNumber, bool getDeleted = false);

        Task<bool> DeleteByOrderNumberAsync(string orderNumber);

        Task<bool> RestoreAsync(string orderNumber);

        Task<bool> CompleteAsync(string orderNumber);

        IEnumerable<string> GetAllOrderNumbers();
    }
}
