namespace Restaurant.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;
    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
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

            this.repository.Setup(r => r.AllAsNoTrackingWithDeleted()).Returns(this.addresses.AsQueryable);

            var result = this.service.IsNameAlreadyInUse(userId, newAddressName, previousAddressName);

            Assert.True(result);
        }


    }
}
