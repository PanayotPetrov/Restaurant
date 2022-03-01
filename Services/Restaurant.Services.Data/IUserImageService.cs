namespace Restaurant.Services.Data
{
    using System.Threading.Tasks;

    using Restaurant.Data.Models;

    public interface IUserImageService
    {
        public Task DeleteAsync(UserImage userImage);
    }
}
