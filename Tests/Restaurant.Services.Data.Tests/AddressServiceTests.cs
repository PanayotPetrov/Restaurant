namespace Restaurant.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels.Address;
    using Xunit;

    [Collection("Mapper assembly")]
    public class AddressServiceTests
    {
        private List<Address> addresses;
        private Mock<IDeletableEntityRepository<Address>> repository;
        private AddressService service;

        public AddressServiceTests()
        {
            this.addresses = new List<Address>();
            this.repository = new Mock<IDeletableEntityRepository<Address>>();
            this.service = new AddressService(this.repository.Object);
        }

        [Theory]
        [InlineData("Lagera", null)]
        [InlineData("Lagera", "HPE")]
        public void IsNameAlreadyTaken_ShouldReturnTrue_IfUserAlreadyHasSuchAnAddress(string newAddressName, string previousAddressName)
        {
            var userId = "Test userId";
            this.addresses.Add(new Address { Name = "Lagera", ApplicationUserId = userId });

            this.addresses.Add(new Address { Name = "HPE", ApplicationUserId = userId });

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.addresses.AsQueryable);

            var result = this.service.IsNameAlreadyInUse(userId, newAddressName, previousAddressName);

            Assert.True(result);
        }

        [Theory]
        [InlineData("Lagera2", null)]
        [InlineData("Lagera2", "HPE")]
        public void IsNameAlreadyTaken_ShouldReturnFalse_IfAddressNameIsAvailable(string newAddressName, string previousAddressName)
        {
            var userId = "Test userId";
            this.addresses.Add(new Address { Name = "Lagera", ApplicationUserId = userId });

            this.addresses.Add(new Address { Name = "HPE", ApplicationUserId = userId });

            this.repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(this.addresses.AsQueryable);

            var result = this.service.IsNameAlreadyInUse(userId, newAddressName, previousAddressName);

            Assert.False(result);
        }

        [Theory]
        [InlineData("Lagera", "Lagera")]
        public void IsNameAlreadyTaken_ShouldReturnFalse_IfAddressNameRemainsUnchanged(string newAddressName, string previousAddressName)
        {
            var userId = "Test userId";
            this.addresses.Add(new Address { Name = "Lagera", ApplicationUserId = userId });

            this.addresses.Add(new Address { Name = "HPE", ApplicationUserId = userId });

            this.repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(this.addresses.AsQueryable);

            var result = this.service.IsNameAlreadyInUse(userId, newAddressName, previousAddressName);

            Assert.False(result);
        }

        [Fact]
        public void GetPrimaryAddressName_ShouldReturnNull_IfUserDoesNotHaveAnyAddresses()
        {
            var userId = "Test userId";

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.addresses.AsQueryable);

            var result = this.service.GetPrimaryAddressName(userId);

            Assert.Null(result);
        }

        [Fact]
        public void GetPrimaryAddressName_ShouldReturnCorrectName()
        {
            var userId = "Test userId";

            this.addresses.Add(new Address { Name = "Lagera", ApplicationUserId = userId, IsPrimaryAddress = true, });

            this.addresses.Add(new Address { Name = "HPE", ApplicationUserId = userId, IsPrimaryAddress = true, IsDeleted = true, });

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.addresses.AsQueryable);

            var result = this.service.GetPrimaryAddressName(userId);

            Assert.Equal("Lagera", result);
        }

        [Fact]
        public void GetByAddressIdAndUserName_ShouldReturnCorrectName()
        {
            var userId = "Test userId";

            this.addresses.Add(new Address
            {
                Name = "Lagera",
                ApplicationUserId = userId,
                ApplicationUser = new ApplicationUser { Id = userId, Addresses = new List<Address>() },
            });
            this.addresses.Add(new Address
            {
                Name = "Hpe",
                ApplicationUserId = "Test userId2",
                ApplicationUser = new ApplicationUser { Id = "Test userId2", Addresses = new List<Address>() },
            });

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.addresses.AsQueryable);

            var result = this.service.GetByUserIdAndAddressName<AddressViewModel>(userId, "Lagera");

            Assert.Equal("Lagera", result.Name);
        }

        [Fact]
        public void GetAddressNamesByUserId_ShouldReturnCorrectNumberOfNames()
        {
            var userId = "Test userId";

            this.addresses.Add(new Address
            {
                Name = "Lagera",
                ApplicationUserId = userId,
            });

            this.addresses.Add(new Address
            {
                Name = "Lagera2",
                ApplicationUserId = userId,
            });
            this.addresses.Add(new Address
            {
                Name = "Hpe",
                ApplicationUserId = "Test userId2",
            });

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.addresses.AsQueryable);

            var result = this.service.GetAddressNamesByUserId(userId);

            Assert.Equal(2, result.Count());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Lagera2")]

        public async Task Delete_ShouldReturnFalseIfInvalidAddressNameProvided(string addressName)
        {
            var userId = "Test userId";

            this.addresses.Add(new Address
            {
                Name = "Lagera",
                ApplicationUserId = userId,
            });

            this.addresses.Add(new Address
            {
                Name = "Hpe",
                ApplicationUserId = "Test userId2",
            });

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.addresses.AsQueryable);

            var result = await this.service.DeleteAsync(addressName);

            Assert.False(result);
        }

        [Fact]

        public async Task Delete_ShouldReturTrueIfAddressNameIsValid()
        {
            var userId = "Test userId";

            this.addresses.Add(new Address
            {
                Name = "Lagera",
                ApplicationUserId = userId,
            });

            this.addresses.Add(new Address
            {
                Name = "Hpe",
                ApplicationUserId = "Test userId2",
            });

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.addresses.AsQueryable);

            var result = await this.service.DeleteAsync("Lagera");

            Assert.True(result);
        }

        [Fact]
        public async Task Delete_ShouldSetAnotherAddressAsPrimaryWhenDeletingPrimaryAddress()
        {
            var userId = "Test userId";

            this.addresses.Add(new Address
            {
                Name = "Lagera",
                ApplicationUserId = userId,
                IsPrimaryAddress = true,
            });

            this.addresses.Add(new Address
            {
                Name = "Hpe",
                ApplicationUserId = userId,
            });

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.addresses.AsQueryable);
            this.repository.Setup(r => r.Delete(It.IsAny<Address>()))
                .Callback((Address address) => this.addresses.Remove(address));

            this.repository.Setup(r => r.All()).Returns(this.addresses.AsQueryable);

            var result = await this.service.DeleteAsync("Lagera");

            Assert.True(this.addresses.Where(a => a.Name == "Hpe").FirstOrDefault().IsPrimaryAddress);
        }

        [Fact]
        public async Task Delete_ShouldDeleteAddress()
        {
            var userId = "Test userId";

            this.addresses.Add(new Address
            {
                Name = "Lagera",
                ApplicationUserId = userId,
                IsPrimaryAddress = true,
            });

            this.addresses.Add(new Address
            {
                Name = "Hpe",
                ApplicationUserId = userId,
            });

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.addresses.AsQueryable);
            this.repository.Setup(r => r.Delete(It.IsAny<Address>()))
                .Callback((Address address) => this.addresses.Remove(address));

            this.repository.Setup(r => r.All()).Returns(this.addresses.AsQueryable);

            await this.service.DeleteAsync("Lagera");

            Assert.Single(this.addresses);
        }

        [Fact]
        public async Task CreateAddressAsync_ShouldCreateAddress()
        {
            var userId = "Test userId";

            var model = new AddAddressModel
            {
                Name = "Lagera",
                Street = "Test street",
                AddressLineTwo = "Test addressLineTwo",
                District = "Lagera",
                City = "Sofia",
                Country = "Bulgaria",
                PostCode = "1000",
                IsPrimaryAddress = true,
            };

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.addresses.AsQueryable);
            this.repository.Setup(r => r.AddAsync(It.IsAny<Address>()))
                .Callback((Address address) => this.addresses.Add(address));

            this.repository.Setup(r => r.All()).Returns(this.addresses.AsQueryable);

            await this.service.CreateNewAddressAsync(model, userId);

            Assert.Single(this.addresses);
        }

        [Fact]
        public async Task CreateAddressAsync_ShouldCreateAddress_AndSetItToPrimary_IfItIsTheOnlyAddress()
        {
            var userId = "Test userId";

            var model = new AddAddressModel
            {
                Name = "Lagera",
                Street = "Test street",
                AddressLineTwo = "Test addressLineTwo",
                District = "Lagera",
                City = "Sofia",
                Country = "Bulgaria",
                PostCode = "1000",
            };

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.addresses.AsQueryable);
            this.repository.Setup(r => r.AddAsync(It.IsAny<Address>()))
                .Callback((Address address) => this.addresses.Add(address));

            this.repository.Setup(r => r.All()).Returns(this.addresses.AsQueryable);

            await this.service.CreateNewAddressAsync(model, userId);

            Assert.True(this.addresses.FirstOrDefault().IsPrimaryAddress);
        }

        [Fact]
        public async Task CreateAddressAsync_ShouldCreateAddress_AndSetItToPrimary_IfMarkedAsSuch()
        {
            var userId = "Test userId";

            this.addresses.Add(new Address { Id = 1, IsPrimaryAddress = true, ApplicationUserId = userId });

            var model = new AddAddressModel
            {
                Name = "Lagera",
                Street = "Test street",
                AddressLineTwo = "Test addressLineTwo",
                District = "Lagera",
                City = "Sofia",
                Country = "Bulgaria",
                PostCode = "1000",
                IsPrimaryAddress = true,
            };

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(this.addresses.AsQueryable);
            this.repository.Setup(r => r.AddAsync(It.IsAny<Address>()))
                .Callback((Address address) => this.addresses.Add(address));

            this.repository.Setup(r => r.All()).Returns(this.addresses.AsQueryable);

            await this.service.CreateNewAddressAsync(model, userId);

            Assert.True(this.addresses.FirstOrDefault(a => a.Name == "Lagera").IsPrimaryAddress);
        }

        [Fact]
        public async Task UpdateAddressAsync_ShouldSetAddressToPrimary_IfMarkedAsSuch()
        {
            var userId = "Test userId";

            this.addresses.Add(new Address
            {
                Id = 1,
                Name = "Lagera",
                IsPrimaryAddress = false,
                ApplicationUserId = userId,
            });

            this.addresses.Add(new Address
            {
                Id = 1,
                Name = "Lagera2",
                IsPrimaryAddress = true,
                ApplicationUserId = userId,
            });

            var model = new AddAddressModel
            {
                Name = "Lagera",
                Street = "Test street",
                AddressLineTwo = "Test addressLineTwo",
                District = "Lagera",
                City = "Sofia",
                Country = "Bulgaria",
                PostCode = "1000",
                IsPrimaryAddress = true,
            };

            this.repository.Setup(r => r.All()).Returns(this.addresses.AsQueryable);

            await this.service.UpdateAddressAsync(model, userId, "Lagera");

            Assert.True(this.addresses.FirstOrDefault(a => a.Name == "Lagera").IsPrimaryAddress);
        }
    }
}
