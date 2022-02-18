namespace Restaurant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;

    public class TableService : ITableService
    {
        private readonly int numberOfTables;

        private readonly IDeletableEntityRepository<Table> tableRepository;

        public TableService(IDeletableEntityRepository<Table> tableRepository)
        {
            this.tableRepository = tableRepository;
            this.numberOfTables = this.AllTables.Count();
        }

        public ICollection<Table> AllTables => this.tableRepository.AllAsNoTracking().ToList();


        public int GetAvailableTableId(DateTime date, int numberOfPeople)
        {
            var bookedTables = this.tableRepository.AllAsNoTracking().Where(t => t.Reservations
            .Any(r => r.ReservationDate.Date == date.Date)).ToList();

            if (bookedTables.Count == this.numberOfTables)
            {
                return -1;
            }

            var availableTables = new List<Table>();

            foreach (var table in this.AllTables)
            {
                if (!bookedTables.Contains(table))
                {
                    availableTables.Add(table);
                }
            }

            return availableTables.OrderBy(t => t.NumberOfPeople).FirstOrDefault().Id;
        }
    }
}
