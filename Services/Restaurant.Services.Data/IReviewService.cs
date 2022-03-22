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

        IEnumerable<T> GetAllWithDeleted<T>(int itemsPerPage, int id);

        public T GetById<T>(int id);

        Task DeleteByIdAsync(int id);
    }
}
