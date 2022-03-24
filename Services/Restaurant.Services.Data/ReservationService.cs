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
        private readonly DateTime currentDate = DateTime.UtcNow.Date;
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

        public T GetByIdWithDeleted<T>(string reservationId)
        {
            return this.reservationRepository.AllAsNoTrackingWithDeleted().Where(x => x.Id == reservationId).To<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetAllByUserId<T>(string userId)
        {
            return this.reservationRepository.AllAsNoTracking().Where(r => r.ApplicationUserId == userId).OrderByDescending(x => x.ReservationDate).To<T>().ToList();
        }

        public int GetCount()
        {
            return this.reservationRepository.AllAsNoTrackingWithDeleted().Where(r => r.ReservationDate.Date >= this.currentDate.Date).Count();
        }

        public IEnumerable<T> GetAllAfterCurrentDate<T>(int itemsPerPage, int page)
        {
            return this.reservationRepository.AllAsNoTrackingWithDeleted().Where(r => r.ReservationDate.Date >= this.currentDate.Date).OrderByDescending(x => x.ReservationDate).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).To<T>().ToList();
        }

        public async Task<string> CreateReservationAsync(AddReservationModel model)
        {
            var tableId = this.CheckForAvailableTable(model.ReservationDate, model.NumberOfPeople);

            var reservation = AutoMapperConfig.MapperInstance.Map<Reservation>(model);
            reservation.TableId = tableId;
            await this.reservationRepository.AddAsync(reservation);
            await this.reservationRepository.SaveChangesAsync();
            return reservation.Id;
        }

        public async Task DeleteByIdAsync(string id)
        {
            var reservation = this.reservationRepository.AllWithDeleted().FirstOrDefault(r => r.Id == id);

            if (reservation.IsDeleted)
            {
                throw new InvalidOperationException("This reservation has already been deleted!");
            }

            this.reservationRepository.Delete(reservation);
            await this.reservationRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(EditReservationModel model)
        {
            var tableId = this.CheckForAvailableTable(model.ReservationDate, model.NumberOfPeople);

            var reservation = this.reservationRepository.AllWithDeleted().FirstOrDefault(r => r.Id == model.Id);
            reservation.ReservationDate = model.ReservationDate;
            reservation.Email = model.Email;
            reservation.NumberOfPeople = model.NumberOfPeople;
            reservation.PhoneNumber = model.PhoneNumber;
            reservation.Fullname = model.FullName;
            reservation.TableId = tableId;
            await this.reservationRepository.SaveChangesAsync();
        }

        public async Task RestoreAsync(string id)
        {
            var reservation = this.reservationRepository.AllWithDeleted().FirstOrDefault(r => r.Id == id);

            if (!reservation.IsDeleted)
            {
                throw new InvalidOperationException("Cannot restore a reservation which has not been deleted!");
            }

            reservation.IsDeleted = false;
            reservation.DeletedOn = null;
            await this.reservationRepository.SaveChangesAsync();
        }

        private int CheckForAvailableTable(DateTime reservationDate, int numberOfPeople)
        {
            var tableId = this.tableService.GetAvailableTableId(reservationDate, numberOfPeople);

            if (tableId == -1)
            {
                throw new InvalidOperationException($"We don't have a table for {numberOfPeople} on the {reservationDate}");
            }

            return tableId;
        }
    }
}
