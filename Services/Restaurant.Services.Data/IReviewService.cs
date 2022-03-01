namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Web.ViewModels.InputModels;

    public interface IReviewService
    {
        IEnumerable<T> GetAllReviews<T>(int itemsPerPage, int page);

        Task AddReviewAsync(AddReviewInputModel model);

        IEnumerable<T> GetLatestFiveReviews<T>();

        int GetCount();
    }
}
