namespace Restaurant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;

    public class TableService : ITableService
    {
        private readonly int numberOfTables;

        private readonly IDeletableEntityRepository<Table> tableRepository;

        public TableService(IDeletableEntityRepository<Table> tableRepository)
        {
            this.tableRepository = tableRepository;
            this.numberOfTables = this.AllTables.Count;
        }

        public ICollection<Table> AllTables => this.tableRepository.AllAsNoTracking().ToList();

        public int GetAvailableTableId(DateTime reservationDate, int numberOfPeople)
        {
            reservationDate = reservationDate.ToUniversalTime();
            var bookedTables = this.tableRepository.AllAsNoTracking().Where(t => t.Reservations
            .Any(r => r.ReservationDate.Date == reservationDate.Date)).ToList();

            if (bookedTables.Count == this.numberOfTables)
            {
                return -1;
            }

            var availableTables = this.AllTables.Except(bookedTables).ToList();

            if (!availableTables.Any(t => t.NumberOfPeople >= numberOfPeople))
            {
                return -1;
            }

            return availableTables.OrderBy(t => t.NumberOfPeople).FirstOrDefault(t => t.NumberOfPeople >= numberOfPeople).Id;
        }
    }
}
