namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Services.Models;

    public interface IReviewService
    {
        IEnumerable<T> GetAllReviews<T>(int itemsPerPage, int page);

        Task AddReviewAsync(AddReviewModel model);

        IEnumerable<T> GetLatestFiveReviews<T>();

        int GetCount();

        int GetCountWithDeleted();

        IEnumerable<T> GetAllWithDeleted<T>(int itemsPerPage, int id);

        T GetById<T>(int id);

        Task<bool> DeleteByIdAsync(int id);

        bool IsReviewIdValid(int reviewId);

        Task<bool> RestoreAsync(int id);

        T GetByIdWithDeleted<T>(int id);
    }
}
