namespace Restaurant.Services.Data
{
    using System.Threading.Tasks;

    using Restaurant.Data.Models;
    using Restaurant.Data.Repositories;

    public class UserImageService : IUserImageService
    {
        private readonly IRestaurantRepositoryDecorator<UserImage> userImageRepository;

        public UserImageService(IRestaurantRepositoryDecorator<UserImage> userImageRepository)
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
