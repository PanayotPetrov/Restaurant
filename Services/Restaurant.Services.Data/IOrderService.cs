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

        IEnumerable<T> GetAllWithDeleted<T>(int itemsPerPage, int id);

        int GetCountWithDeleted();

        T GetByOrderNumberWithDeleted<T>(string orderNumber);

        Task<bool> DeleteByIdAsync(string orderNumber);

        Task<bool> RestoreAsync(string orderNumber);

        Task<bool> CompleteAsync(string orderNumber);

        IEnumerable<string> GetAllOrderNumbers();
    }
}
