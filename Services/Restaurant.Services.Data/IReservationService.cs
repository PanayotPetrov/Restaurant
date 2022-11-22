namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Services.Models;

    public interface IReservationService : IPaginationService
    {
        T GetById<T>(string reservationId, bool getDeleted = false);

        IEnumerable<T> GetAllByUserId<T>(string userId);

        Task<string> CreateReservationAsync(AddReservationModel model);

        Task<bool> DeleteByIdAsync(string id);

        Task<bool> UpdateAsync(EditReservationModel model);

        Task<bool> RestoreAsync(string id);

        bool IsReservationIdValid(string id);
    }
}
