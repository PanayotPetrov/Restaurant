namespace Restaurant.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Xunit;

    public class UserImageServiceTests
    {
        [Fact]
        public async Task DeleteAsync_ShouldDeleteUserImage()
        {
            var repository = new Mock<IRepository<UserImage>>();
            var service = new UserImageService(repository.Object);
            var userImages = new List<UserImage>
            {
                new UserImage { Id = "Test user image id 1", ApplicationUserId = "Test user id 1" },
                new UserImage { Id = "Test user image id 2", ApplicationUserId = "Test user id 2" },
                new UserImage { Id = "Test user image id 3", ApplicationUserId = "Test user id 3" },
            };

            repository.Setup(x => x.Delete(It.IsAny<UserImage>())).Callback((UserImage userImage) => userImages.Remove(userImage));
            await service.DeleteAsync(userImages.FirstOrDefault());
            Assert.Equal(2, userImages.Count);
        }
    }
}
