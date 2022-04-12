namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Services.Models;

    public interface IReservationService
    {
        T GetById<T>(string reservationId);

        T GetByIdWithDeleted<T>(string reservationId);

        IEnumerable<T> GetAllByUserId<T>(string userId);

        Task<string> CreateReservationAsync(AddReservationModel model);

        Task<bool> DeleteByIdAsync(string id);

        int GetCount();

        IEnumerable<T> GetAllWithoutPassedDates<T>(int itemsPerPage, int page);

        Task<bool> UpdateAsync(EditReservationModel model);

        Task<bool> RestoreAsync(string id);

        bool IsReservationIdValid(string id);
    }
}
