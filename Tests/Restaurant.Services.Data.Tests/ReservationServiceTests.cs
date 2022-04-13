namespace Restaurant.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels.Reservation;
    using Xunit;

    [Collection("Mapper assembly")]
    public class ReservationServiceTests
    {
        private List<Reservation> reservations;
        private Mock<IDeletableEntityRepository<Reservation>> repository;
        private Mock<ITableService> tableService;
        private ReservationService service;

        public ReservationServiceTests()
        {
            this.reservations = new List<Reservation>();
            this.repository = new Mock<IDeletableEntityRepository<Reservation>>();
            this.tableService = new Mock<ITableService>();
            this.service = new ReservationService(this.repository.Object, this.tableService.Object);
        }

        [Fact]
        public void IsValid_ShouldReturnFalseIfReservationIdIsInvalid()
        {
            this.reservations.Add(new Reservation
            {
                Id = "Valid id",
            });

            this.repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(this.reservations.AsQueryable());

            Assert.False(this.service.IsReservationIdValid("Invalid Id"));
        }

        [Fact]
        public void IsValid_ShouldReturnTrueIfReservationIdIsInvalid()
        {
            this.reservations.Add(new Reservation
            {
                Id = "Valid id",
            });

            this.repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(this.reservations.AsQueryable());

            Assert.True(this.service.IsReservationIdValid("Valid id"));
        }

        [Fact]
        public async Task RestoreAsync_ShouldReturnFalseIfReservationIsNotDeleted()
        {
            this.reservations.Add(new Reservation
            {
                Id = "Valid id",
                IsDeleted = false,
            });

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.reservations.AsQueryable());
            Assert.False(await this.service.RestoreAsync("Valid id"));
        }

        [Fact]
        public async Task RestoreAsync_ShouldReturnTrueIfReservationIsRestored()
        {
            this.reservations.Add(new Reservation
            {
                Id = "Valid id",
                IsDeleted = true,
            });

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.reservations.AsQueryable());
            Assert.True(await this.service.RestoreAsync("Valid id"));
        }

        [Fact]
        public async Task RestoreAsync_ShouldRestoreReservation()
        {
            var reservation = new Reservation
            {
                Id = "Test id",
                IsDeleted = true,
            };
            this.reservations.Add(reservation);

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.reservations.AsQueryable());
            await this.service.RestoreAsync(reservation.Id);
            Assert.False(this.reservations.FirstOrDefault().IsDeleted);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldReturnTrueIfSuccessfullyDeleted()
        {
            this.reservations.Add(new Reservation
            {
                Id = "Valid id",
                IsDeleted = false,
            });

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.reservations.AsQueryable());
            Assert.True(await this.service.DeleteByIdAsync("Valid id"));
        }

        [Fact]
        public async Task DeleteById_ShouldDeleteReservation()
        {
            var reservation = new Reservation
            {
                Id = "Test id",
                IsDeleted = false,
            };
            this.reservations.Add(reservation);

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.reservations.AsQueryable());
            this.repository.Setup(r => r.Delete(It.IsAny<Reservation>())).Callback((Reservation reservation) => this.reservations.Remove(reservation));
            await this.service.DeleteByIdAsync(reservation.Id);
            Assert.Empty(this.reservations);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldReturnFalseIfReservationAlreadyDeleted()
        {
            this.reservations.Add(new Reservation
            {
                Id = "Valid id",
                IsDeleted = true,
            });

            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.reservations.AsQueryable());
            Assert.False(await this.service.DeleteByIdAsync("Valid id"));
        }

        [Fact]
        public async Task CreateReservationAsync_ShouldReturnNullIfNoTableIsAvailable()
        {
            var addreservationModel = new AddReservationModel
            {
                ReservationDate = DateTime.UtcNow,
                NumberOfPeople = 1,
            };

            this.tableService.Setup(t => t.GetAvailableTableId(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(-1);
            Assert.Null(await this.service.CreateReservationAsync(addreservationModel));
        }

        [Fact]
        public async Task CreateReservationAsync_ShouldReturnCorrectReservationIdIfSuccessfullyCreated()
        {
            var addreservationModel = new AddReservationModel
            {
                ReservationDate = DateTime.UtcNow,
                NumberOfPeople = 1,
            };

            this.tableService.Setup(t => t.GetAvailableTableId(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(1);
            Assert.NotNull(await this.service.CreateReservationAsync(addreservationModel));
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalseIfNoTableIsAvailableForNewDate()
        {
            var addreservationModel = new EditReservationModel
            {
                ReservationDate = DateTime.UtcNow,
                NumberOfPeople = 1,
            };

            this.tableService.Setup(t => t.GetAvailableTableId(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(-1);
            Assert.False(await this.service.UpdateAsync(addreservationModel));
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrueIfReservationUpdatedSuccessfully()
        {
            this.reservations.Add(new Reservation { Id = "Test id" });

            var addreservationModel = new EditReservationModel
            {
                Id = "Test id",
                FullName = "Test name",
                PhoneNumber = "0888 111 222",
                Email = "test@abv.bg",
                SpecialRequest = "Test request",
                ApplicationUserId = "Test user id",
                ReservationTime = 18,
                ReservationDate = DateTime.UtcNow,
                NumberOfPeople = 1,
            };

            this.tableService.Setup(t => t.GetAvailableTableId(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(1);
            this.repository.Setup(r => r.AllWithDeleted()).Returns(this.reservations.AsQueryable());
            Assert.True(await this.service.UpdateAsync(addreservationModel));
        }

        [Fact]
        public void GetAllWithoutPassedDates_ShouldReturnCorrectReservations()
        {
            this.reservations.Add(new Reservation { Id = "Test id1", ReservationDate = DateTime.UtcNow });
            this.reservations.Add(new Reservation { Id = "Test id2", ReservationDate = DateTime.UtcNow.AddDays(1) });
            this.reservations.Add(new Reservation { Id = "Test id", ReservationDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)) });

            this.repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(this.reservations.AsQueryable());
            var result = this.service.GetAllWithoutPassedDates<AdminReservationViewModel>(3, 1).ToArray();
            Assert.Equal(2, result.Count());
            Assert.Equal("Test id2", result[0].Id);
            Assert.Equal("Test id1", result[1].Id);
        }

        [Fact]
        public void GetAllWithoutPassedDates_ShouldReturnCorrectAmountOfReservations()
        {
            this.reservations.Add(new Reservation { Id = "Test id", ReservationDate = DateTime.UtcNow });
            this.reservations.Add(new Reservation { Id = "Test id", ReservationDate = DateTime.UtcNow.AddDays(1) });
            this.reservations.Add(new Reservation { Id = "Test id", ReservationDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)) });
            this.reservations.Add(new Reservation { Id = "Test id", ReservationDate = DateTime.UtcNow });
            this.reservations.Add(new Reservation { Id = "Test id", ReservationDate = DateTime.UtcNow.AddDays(1) });
            this.reservations.Add(new Reservation { Id = "Test id", ReservationDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)) });

            this.repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(this.reservations.AsQueryable());
            var result1 = this.service.GetAllWithoutPassedDates<ReservationViewModel>(3, 1).Count();
            var result2 = this.service.GetAllWithoutPassedDates<ReservationViewModel>(3, 2).Count();

            Assert.Equal(3, result1);
            Assert.Equal(1, result2);
        }

        [Fact]
        public void GetCount_ShouldReturnCorrectAmountOfReservations()
        {
            this.reservations.Add(new Reservation { Id = "Test id", ReservationDate = DateTime.UtcNow });
            this.reservations.Add(new Reservation { Id = "Test id", ReservationDate = DateTime.UtcNow.AddDays(1) });
            this.reservations.Add(new Reservation { Id = "Test id", ReservationDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)) });
            this.reservations.Add(new Reservation { Id = "Test id", ReservationDate = DateTime.UtcNow });
            this.reservations.Add(new Reservation { Id = "Test id", ReservationDate = DateTime.UtcNow.AddDays(1) });
            this.reservations.Add(new Reservation { Id = "Test id", ReservationDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)) });

            this.repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(this.reservations.AsQueryable());
            var result = this.service.GetCount();

            Assert.Equal(4, result);
        }

        [Fact]
        public void GetAllByUserId_ShouldReturnCorrectReservations()
        {
            this.reservations.Add(new Reservation { ApplicationUserId = "Test UserId1", ReservationDate = DateTime.UtcNow });
            this.reservations.Add(new Reservation { ApplicationUserId = "Test UserId1", ReservationDate = DateTime.UtcNow });
            this.reservations.Add(new Reservation { ApplicationUserId = "Test UserId1", ReservationDate = DateTime.UtcNow });
            this.reservations.Add(new Reservation { ApplicationUserId = "Test UserId2", ReservationDate = DateTime.UtcNow });
            this.reservations.Add(new Reservation { ApplicationUserId = "Test UserId3", ReservationDate = DateTime.UtcNow });
            this.reservations.Add(new Reservation { ApplicationUserId = "Test UserId4", ReservationDate = DateTime.UtcNow });

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.reservations.AsQueryable());
            var result = this.service.GetAllByUserId<AdminReservationViewModel>("Test UserId1");

            foreach (var reservation in result)
            {
                Assert.Equal("Test UserId1", reservation.ApplicationUserId);
            }
        }

        [Fact]
        public void GetByIdWithDeleted_ShouldReturnCorrectReservation()
        {
            this.reservations.Add(new Reservation { Id = "Test Id1" });
            this.reservations.Add(new Reservation { Id = "Test Id2" });

            this.repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(this.reservations.AsQueryable());
            var result = this.service.GetByIdWithDeleted<AdminReservationViewModel>("Test Id1");

            Assert.Equal("Test Id1", result.Id);
        }

        [Fact]
        public void GetById_ShouldReturnCorrectReservation()
        {
            this.reservations.Add(new Reservation { Id = "Test Id1" });
            this.reservations.Add(new Reservation { Id = "Test Id2" });

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.reservations.AsQueryable());
            var result = this.service.GetById<AdminReservationViewModel>("Test Id1");

            Assert.Equal("Test Id1", result.Id);
        }
    }
}
