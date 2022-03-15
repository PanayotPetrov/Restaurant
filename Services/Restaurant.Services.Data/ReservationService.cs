namespace Restaurant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;

    public class ReservationService : IReservationService
    {
        private readonly IDeletableEntityRepository<Reservation> reservationRepository;
        private readonly ITableService tableService;

        public ReservationService(IDeletableEntityRepository<Reservation> reservationRepository, ITableService tableService)
        {
            this.reservationRepository = reservationRepository;
            this.tableService = tableService;
        }

        public T GetById<T>(string reservationId)
        {
            return this.reservationRepository.AllAsNoTracking().Where(x => x.Id == reservationId).To<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetAllByUserId<T>(string userId)
        {
            return this.reservationRepository.AllAsNoTracking().Where(r => r.ApplicationUserId == userId).OrderByDescending(x => x.ReservationDate).To<T>().ToList();
        }

        public async Task<string> CreateReservationAsync(AddReservationModel model)
        {
            var tableId = this.tableService.GetAvailableTableId(model.ReservationDate, model.NumberOfPeople);

            if (tableId == -1)
            {
                throw new InvalidOperationException($"We don't have a table for {model.NumberOfPeople} on the {model.ReservationDate}");
            }

            var reservation = AutoMapperConfig.MapperInstance.Map<Reservation>(model);
            reservation.TableId = tableId;
            await this.reservationRepository.AddAsync(reservation);
            await this.reservationRepository.SaveChangesAsync();
            return reservation.Id;
        }
    }
}
