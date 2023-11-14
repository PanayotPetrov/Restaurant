namespace Restaurant.Services.Data
{
    using System;
    using System.Linq;

    using Restaurant.Data.Models;
    using Restaurant.Data.Repositories;

    public class TableService : ITableService
    {
        private readonly IRestaurantDeletableEntityRepositoryDecorator<Table> tableRepository;

        public TableService(IRestaurantDeletableEntityRepositoryDecorator<Table> tableRepository)
        {
            this.tableRepository = tableRepository;
        }

        public int GetAvailableTableId(DateTime reservationDate, int numberOfPeople)
        {
            var numberOfTables = this.tableRepository.AllAsNoTracking().Count();
            reservationDate = reservationDate.ToUniversalTime();

            var bookedTables = this.tableRepository.AllAsNoTracking().Where(t => t.Reservations
            .Any(r => r.ReservationDate.Date == reservationDate.Date)).ToList();

            if (bookedTables.Count == numberOfTables)
            {
                return -1;
            }

            var availableTables = this.tableRepository.AllAsNoTracking().ToList().Except(bookedTables).ToList();

            if (!availableTables.Any(t => t.NumberOfPeople >= numberOfPeople))
            {
                return -1;
            }

            return availableTables.OrderBy(t => t.NumberOfPeople).FirstOrDefault(t => t.NumberOfPeople >= numberOfPeople).Id;
        }
    }
}
