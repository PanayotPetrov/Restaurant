namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Services.Models;

    public interface IReservationService
    {
        T GetById<T>(string reservationId);

        IEnumerable<T> GetAllByUserId<T>(string userId);

        Task<string> CreateReservationAsync(AddReservationModel model);
    }
}
