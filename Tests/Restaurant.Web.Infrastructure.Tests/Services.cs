namespace Restaurant.Web.Infrastructure.Tests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;
    using Moq;
    using Restaurant.Services.Data;

    public class Services
    {
        public Services()
        {
            this.ServiceCollection = new Mock<IServiceProvider>();
            this.AddressServiceMock = new Mock<IAddressService>();
            this.CartServiceMock = new Mock<ICartService>();
            this.ReservationServiceMock = new Mock<IReservationService>();
            this.OrderServiceMock = new Mock<IOrderService>();
            this.MealServiceMock = new Mock<IMealService>();
            this.ReviewServiceMock = new Mock<IReviewService>();

            this.ValidationContext = new ValidationContext("null", this.ServiceCollection.Object, null);
            this.HttpContextAccessorMock = new Mock<IHttpContextAccessor>();
            this.HttpContext = new DefaultHttpContext();
            this.ClaimsPrincipalMock = new Mock<ClaimsPrincipal>();
            this.UserIdClaim = new Claim("Test claim", "Test userId");

            this.ServiceCollection.Setup(x => x.GetService(typeof(IHttpContextAccessor)))
                    .Returns(this.HttpContextAccessorMock.Object);
            this.ServiceCollection.Setup(x => x.GetService(typeof(IAddressService)))
                    .Returns(this.AddressServiceMock.Object);
            this.ServiceCollection.Setup(x => x.GetService(typeof(ICartService)))
                    .Returns(this.CartServiceMock.Object);
            this.ServiceCollection.Setup(x => x.GetService(typeof(IReservationService)))
                    .Returns(this.ReservationServiceMock.Object);
            this.ServiceCollection.Setup(x => x.GetService(typeof(IOrderService)))
                    .Returns(this.OrderServiceMock.Object);
            this.ServiceCollection.Setup(x => x.GetService(typeof(IMealService)))
                    .Returns(this.MealServiceMock.Object);
            this.ServiceCollection.Setup(x => x.GetService(typeof(IReviewService)))
                    .Returns(this.ReviewServiceMock.Object);

            this.HttpContextAccessorMock.Setup(x => x.HttpContext).Returns(this.HttpContext);
            this.ClaimsPrincipalMock.Setup(x => x.FindFirst(It.IsAny<string>())).Returns(this.UserIdClaim);
            this.HttpContext.User = this.ClaimsPrincipalMock.Object;
        }

        public Mock<IServiceProvider> ServiceCollection { get; private set; }

        public Mock<IAddressService> AddressServiceMock { get; private set; }

        public Mock<ICartService> CartServiceMock { get; private set; }

        public Mock<IReservationService> ReservationServiceMock { get; private set; }

        public Mock<IOrderService> OrderServiceMock { get; private set; }

        public Mock<IMealService> MealServiceMock { get; private set; }

        public Mock<IReviewService> ReviewServiceMock { get; private set; }

        public ValidationContext ValidationContext { get; private set; }

        public HttpContext HttpContext { get; private set; }

        public Mock<IHttpContextAccessor> HttpContextAccessorMock { get; private set; }

        public Mock<ClaimsPrincipal> ClaimsPrincipalMock { get; private set; }

        public Claim UserIdClaim { get; private set; }
    }
}
