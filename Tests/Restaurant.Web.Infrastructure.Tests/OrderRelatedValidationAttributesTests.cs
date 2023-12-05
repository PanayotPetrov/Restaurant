namespace Restaurant.Web.Infrastructure.Tests
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AdminDashboard.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using Restaurant.Data.Models;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Xunit;

    [Collection("Services")]
    public class OrderRelatedValidationAttributesTests
    {
        private Services fixture;

        public OrderRelatedValidationAttributesTests(Services fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void OrderNumberValidation_IsValid_ShouldReturnTrueIfExists()
        {
            var orderNumber = "Test order number";

            var orderNumbers = new List<string>
            {
                "Test order number",
                "Test order number2",
                "Test order number3",
            };

            var user = new ApplicationUser();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var mgr = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            mgr.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).Returns(Task.FromResult(user));
            mgr.Setup(um => um.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns(Task.FromResult(false));

            this.fixture.ServiceCollection.Setup(um => um.GetService(typeof(UserManager<ApplicationUser>))).Returns(mgr.Object);

            this.fixture.OrderServiceMock.Setup(x => x.GetAllOrderNumbersByUserId(It.IsAny<string>())).Returns(orderNumbers);

            var attribute = new OrderNumberValidationAttribute();
            var result = attribute.GetValidationResult(orderNumber, this.fixture.ValidationContext);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void OrderNumberValidation_IsValid_ShouldReturnTrueIfExistsAndUserIsAdmin()
        {
            var orderNumber = "Test order number";

            var orderNumbers = new List<string>
            {
                "Test order number",
                "Test order number2",
                "Test order number3",
            };

            var user = new AdminDashboardUser();

            var store = new Mock<IUserStore<AdminDashboardUser>>();
            var mgr = new Mock<UserManager<AdminDashboardUser>>(store.Object, null, null, null, null, null, null, null, null);

            mgr.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).Returns(Task.FromResult(user));

            this.fixture.ServiceCollection.Setup(um => um.GetService(typeof(UserManager<AdminDashboardUser>))).Returns(mgr.Object);

            this.fixture.OrderServiceMock.Setup(x => x.GetAllOrderNumbers()).Returns(orderNumbers);

            var attribute = new OrderNumberValidationAttribute();
            var result = attribute.GetValidationResult(orderNumber, this.fixture.ValidationContext);

            Assert.Equal(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData("Test order number")]
        [InlineData(null)]

        public void OrderNumberValidation_IsValid_ShouldReturnFalse_IfOrderNumberDoesNotExit(string orderNumber)
        {
            var orderNumbers = new List<string>
            {
                "Test order number1",
                "Test order number2",
                "Test order number3",
            };

            var user = new ApplicationUser();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var mgr = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            mgr.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).Returns(Task.FromResult(user));

            this.fixture.ServiceCollection.Setup(um => um.GetService(typeof(UserManager<ApplicationUser>))).Returns(mgr.Object);

            this.fixture.OrderServiceMock.Setup(x => x.GetAllOrderNumbersByUserId(It.IsAny<string>())).Returns(orderNumbers);

            var attribute = new OrderNumberValidationAttribute();
            var result = attribute.GetValidationResult(orderNumber, this.fixture.ValidationContext);

            Assert.NotEqual(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData("Test order number")]
        [InlineData(null)]
        public void OrderNumberValidation_IsValid_ShouldReturnFalse_IfOrderNumberDoesNotExitAndUserIsAdmin(string orderNumber)
        {
            var orderNumbers = new List<string>
            {
                "Test order number1",
                "Test order number2",
                "Test order number3",
            };

            var user = new AdminDashboardUser();

            var store = new Mock<IUserStore<AdminDashboardUser>>();
            var mgr = new Mock<UserManager<AdminDashboardUser>>(store.Object, null, null, null, null, null, null, null, null);

            mgr.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).Returns(Task.FromResult(user));

            this.fixture.ServiceCollection.Setup(um => um.GetService(typeof(UserManager<AdminDashboardUser>))).Returns(mgr.Object);

            this.fixture.OrderServiceMock.Setup(x => x.GetAllOrderNumbers()).Returns(orderNumbers);

            var attribute = new OrderNumberValidationAttribute();
            var result = attribute.GetValidationResult(orderNumber, this.fixture.ValidationContext);

            Assert.NotEqual(ValidationResult.Success, result);
        }
    }
}
