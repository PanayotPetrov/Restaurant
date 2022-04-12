namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Restaurant.Services.Models;

    public interface IAddressService
    {
        public Task UpdateAddressAsync(AddAddressModel model, string userId, string addressName);

        public Task CreateNewAddressAsync(AddAddressModel model, string userId);

        public T GetByUserIdAndAddressName<T>(string userId, string addressName);

        public Task<bool> DeleteAsync(string addressName);

        public string GetPrimaryAddressName(string userId);

        public bool IsNameAlreadyInUse(string userId, string addressName, string originalAddressName);

        public IEnumerable<string> GetAddressNamesByUserId(string userId);

        public IEnumerable<string> GetAllowedDistricts();
    }
}
