namespace Restaurant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Restaurant.Data.Models;
    using Restaurant.Data.Repositories;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;

    public class ReservationService : PaginationService<Reservation>, IReservationService
    {
        private readonly DateTime currentDate = DateTime.UtcNow.Date;
        private readonly IRestaurantDeletableEntityRepositoryDecorator<Reservation> reservationRepository;
        private readonly ITableService tableService;

        public ReservationService(IRestaurantDeletableEntityRepositoryDecorator<Reservation> reservationRepository, ITableService tableService)
            : base(reservationRepository)
        {
            this.reservationRepository = reservationRepository;
            this.tableService = tableService;
        }

        public T GetById<T>(string reservationId, bool getDeleted = false)
        {
            return getDeleted
                ? this.reservationRepository.AllAsNoTrackingWithDeleted().Where(x => x.Id == reservationId).To<T>().FirstOrDefault()
                : this.reservationRepository.AllAsNoTracking().Where(x => x.Id == reservationId).To<T>().FirstOrDefault();
        }

        public override int GetCount(bool getDeleted = false)
        {
            return getDeleted == true
                ? this.reservationRepository.AllAsNoTrackingWithDeleted().Where(r => r.ReservationDate.Date >= this.currentDate.Date).Count()
                : this.reservationRepository.AllAsNoTracking().Where(r => r.ReservationDate.Date >= this.currentDate.Date).Count();
        }

        public IEnumerable<T> GetAllByUserId<T>(string userId)
        {
            return this.reservationRepository
                .AllAsNoTracking()
                .Where(r => r.ApplicationUserId == userId)
                .OrderByDescending(x => x.ReservationDate)
                .To<T>()
                .ToList();
        }

        public override IEnumerable<T> GetAllWithPagination<T>(int itemsPerPage, int page, bool getDeleted = false)
        {
            var itemsToSkip = this.GetItemsToSkip(itemsPerPage, page, getDeleted);

            if (getDeleted)
            {
                return this.reservationRepository
                    .AllAsNoTrackingWithDeleted()
                    .Where(r => r.ReservationDate.Date >= this.currentDate.Date)
                    .OrderByDescending(x => x.ReservationDate)
                    .Skip(itemsToSkip)
                    .Take(itemsPerPage)
                    .To<T>()
                    .ToList();
            }

            return this.reservationRepository
                .AllAsNoTracking()
                .Where(r => r.ReservationDate.Date >= this.currentDate.Date)
                .OrderByDescending(x => x.ReservationDate)
                .Skip(itemsToSkip)
                .Take(itemsPerPage)
                .To<T>()
                .ToList();
        }

        public async Task<string> CreateReservationAsync(AddReservationModel model)
        {
            var tableId = this.GetAvailableTableId(model.ReservationDate, model.NumberOfPeople);
            if (tableId == -1)
            {
                return null;
            }

            var reservation = AutoMapperConfig.MapperInstance.Map<Reservation>(model);
            reservation.TableId = tableId;
            await this.reservationRepository.AddAsync(reservation);
            await this.reservationRepository.SaveChangesAsync();
            return reservation.Id;
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            var reservation = this.reservationRepository.AllWithDeleted().FirstOrDefault(r => r.Id == id);

            if (reservation is null)
            {
                throw new NullReferenceException();
            }

            if (reservation.IsDeleted)
            {
                return false;
            }

            this.reservationRepository.Delete(reservation);
            await this.reservationRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(EditReservationModel model)
        {
            var tableId = this.GetAvailableTableId(model.ReservationDate, model.NumberOfPeople);

            if (tableId == -1)
            {
                return false;
            }

            var reservation = this.reservationRepository.AllWithDeleted().FirstOrDefault(r => r.Id == model.Id);
            reservation.ReservationDate = model.ReservationDate;
            reservation.Email = model.Email;
            reservation.NumberOfPeople = model.NumberOfPeople;
            reservation.PhoneNumber = model.PhoneNumber;
            reservation.Fullname = model.FullName;
            reservation.TableId = tableId;
            await this.reservationRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreAsync(string id)
        {
            var reservation = this.reservationRepository.AllWithDeleted().FirstOrDefault(r => r.Id == id);

            if (reservation is null)
            {
                throw new NullReferenceException();
            }

            if (!reservation.IsDeleted)
            {
                return false;
            }

            reservation.IsDeleted = false;
            reservation.DeletedOn = null;
            await this.reservationRepository.SaveChangesAsync();
            return true;
        }

        public bool IsReservationIdValid(string reservationId)
        {
            return this.reservationRepository.AllAsNoTrackingWithDeleted().Any(r => r.Id == reservationId);
        }

        private int GetAvailableTableId(DateTime reservationDate, int numberOfPeople)
        {
            var tableId = this.tableService.GetAvailableTableId(reservationDate, numberOfPeople);

            return tableId;
        }
    }
}
