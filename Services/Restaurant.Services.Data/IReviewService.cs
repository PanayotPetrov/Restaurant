namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Services.Models;

    public interface IReviewService : IPaginationService
    {
        Task AddReviewAsync(AddReviewModel model);

        IEnumerable<T> GetLatestFiveReviews<T>();

        T GetById<T>(int id, bool getDeleted = false);

        Task<bool> DeleteByIdAsync(int id);

        bool IsReviewIdValid(int reviewId);

        Task<bool> RestoreAsync(int id);
    }
}
