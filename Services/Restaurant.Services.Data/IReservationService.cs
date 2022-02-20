namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Web.ViewModels.InputModels;

    public interface IReservationService
    {
        T GetById<T>(string reservationId);

        IEnumerable<T> GetAllByUserId<T>(string userId);

        Task<string> CreateReservationAsync(AddReservationInputModel model);
    }
}
