namespace Restaurant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Web.ViewModels.InputModels;

    public class ReservationService : IReservationService
    {
        public T GetReservation<T>(int reservationId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAllByUserId<T>(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateReservationAsync(AddReservationInputModel model)
        {
            throw new NotImplementedException();
        }
    }
}
