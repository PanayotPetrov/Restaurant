namespace Restaurant.Services.Data
{
    using System.Threading.Tasks;

    using Restaurant.Services.Models;

    public interface IAddressService
    {
        public Task UpdateAddressAsync(AddAddressModel model, string userId, string addressName);

        public Task CreateNewAddressAsync(AddAddressModel model, string userId);

        public T GetByNameAndUserId<T>(string userId, string addressName);

        public Task DeleteAsync(string addressName);

        public T GetPrimaryAddress<T>(string userId);
    }
}
