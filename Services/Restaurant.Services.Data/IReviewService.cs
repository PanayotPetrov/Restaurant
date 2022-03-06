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
    }
}
