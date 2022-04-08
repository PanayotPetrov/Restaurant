namespace Restaurant.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Moq;
    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Xunit;

    public class TableServiceTests
    {
        private List<Table> tables;
        private Mock<IDeletableEntityRepository<Table>> repository;
        private TableService service;

        public TableServiceTests()
        {
            this.tables = new List<Table>();
            this.repository = new Mock<IDeletableEntityRepository<Table>>();
            this.service = new TableService(this.repository.Object);
        }

        [Fact]
        public void GetAvailableTableId_ShouldReturnMinusOneIfAllTablesAreBooked()
        {
            for (int i = 0; i < 6; i++)
            {
                var table = new Table
                {
                    Id = i,
                    NumberOfPeople = 6,
                    Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            ReservationDate = DateTime.UtcNow.AddDays(1),
                            TableId = i,
                        },
                    },
                };
                this.tables.Add(table);
            }

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.tables.AsQueryable());

            var result = this.service.GetAvailableTableId(DateTime.Now.AddDays(1), 6);
            Assert.Equal(-1, result);
        }

        [Fact]
        public void GetAvailableTableId_ShouldReturnMinusOneIfTableIsAvailableForNumberOfPeople()
        {
            for (int i = 0; i < 6; i++)
            {
                var table = new Table
                {
                    Id = i,
                    NumberOfPeople = 6 - i,
                };
                if (i < 3)
                {
                    table.Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            ReservationDate = DateTime.UtcNow.AddDays(1),
                            TableId = i,
                        },
                    };
                }

                this.tables.Add(table);
            }

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.tables.AsQueryable());
            var result = this.service.GetAvailableTableId(DateTime.Now.AddDays(1), 6);
            Assert.Equal(-1, result);
        }

        [Fact]
        public void GetAvailableTableId_ShouldReturnTableIdWithSmallestNumberOfPeople()
        {
            for (int i = 0; i < 6; i++)
            {
                var table = new Table
                {
                    Id = i,
                    NumberOfPeople = 6 - i,
                };
                if (i < 3)
                {
                    table.Reservations = new List<Reservation>
                    {
                        new Reservation
                        {
                            ReservationDate = DateTime.UtcNow.AddDays(1),
                            TableId = i,
                        },
                    };
                }

                this.tables.Add(table);
            }

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.tables.AsQueryable());
            var result = this.service.GetAvailableTableId(DateTime.Now.AddDays(1), 1);
            Assert.Equal(5, result);
        }
    }
}
