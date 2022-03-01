namespace Restaurant.Services.Data
{
    using System.Threading.Tasks;

    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;

    public class UserImageService : IUserImageService
    {
        private readonly IRepository<UserImage> userImageRepository;

        public UserImageService(IRepository<UserImage> userImageRepository)
        {
            this.userImageRepository = userImageRepository;
        }

        public async Task DeleteAsync(UserImage userImage)
        {
            this.userImageRepository.Delete(userImage);
            await this.userImageRepository.SaveChangesAsync();
        }
    }
}
