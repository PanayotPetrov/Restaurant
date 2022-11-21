namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Services.Models;

    public interface IReviewService
    {
        Task AddReviewAsync(AddReviewModel model);

        IEnumerable<T> GetLatestFiveReviews<T>();

        int GetCount(bool getDeleted = false);

        IEnumerable<T> GetAllWithPagination<T>(int itemsPerPage, int id, bool getDeleted = false);

        T GetById<T>(int id, bool getDeleted = false);

        Task<bool> DeleteByIdAsync(int id);

        bool IsReviewIdValid(int reviewId);

        Task<bool> RestoreAsync(int id);
    }
}
