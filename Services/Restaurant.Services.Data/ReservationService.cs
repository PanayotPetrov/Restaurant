namespace Restaurant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Web.ViewModels.InputModels;

    public class ReservationService : IReservationService
    {
        private readonly IDeletableEntityRepository<Reservation> reservationRepository;
        private readonly ITableService tableService;

        public ReservationService(IDeletableEntityRepository<Reservation> reservationRepository, ITableService tableService)
        {
            this.reservationRepository = reservationRepository;
            this.tableService = tableService;
        }

        public T GetById<T>(int reservationId)
        {
            return this.reservationRepository.AllAsNoTracking().Where(x => x.Id == reservationId).To<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetAllByUserId<T>(string userId)
        {
            return this.reservationRepository.AllAsNoTracking().Where(r => r.ApplicationUserId == userId).To<T>().ToList();
        }

        public async Task<int> CreateReservationAsync(AddReservationInputModel model)
        {
            var tableId = this.tableService.GetAvailableTableId(model.ReservationDate, model.NumberOfPeople);

            if (tableId == -1)
            {
                throw new InvalidOperationException($"We don't have a table for {model.NumberOfPeople} on the {model.ReservationDate}");
            }

            var reservation = new Reservation
            {
                ApplicationUserId = model.UserId,
                Fullname = $"{model.FirstName} {model.LastName}",
                ReservationDate = model.ReservationDate.ToUniversalTime(),
                NumberOfPeople = model.NumberOfPeople,
                TableId = tableId,
                Email = model.Email,
            };
            await this.reservationRepository.AddAsync(reservation);
            await this.reservationRepository.SaveChangesAsync();
            return reservation.Id;
        }
    }
}
