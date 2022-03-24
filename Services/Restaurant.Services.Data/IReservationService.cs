﻿namespace Restaurant.Services.Data
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

        Task DeleteByIdAsync(string id);

        int GetCount();

        IEnumerable<T> GetAllAfterCurrentDate<T>(int itemsPerPage, int page);

        Task UpdateAsync(EditReservationModel model);

        Task RestoreAsync(string id);
    }
}
