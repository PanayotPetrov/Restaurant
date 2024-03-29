﻿namespace Restaurant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Restaurant.Data.Models;
    using Restaurant.Data.Repositories;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;

    public class AddressService : IAddressService
    {
        private readonly IRestaurantDeletableEntityRepositoryDecorator<Address> addressRepository;
        private readonly IEnumerable<string> allowedDistricts = new List<string> { "Lagera", "Hipodruma", "Centur", "Zona-B5", "Serdika", "Lozenets" };

        public AddressService(IRestaurantDeletableEntityRepositoryDecorator<Address> addressRepository)
        {
            this.addressRepository = addressRepository;
        }

        public T GetByUserIdAndAddressName<T>(string userId, string addressName)
        {
            return this.addressRepository.AllAsNoTracking().Where(a => a.ApplicationUserId == userId && a.Name == addressName).To<T>().FirstOrDefault();
        }

        public string GetPrimaryAddressName(string userId)
        {
            return this.addressRepository.AllAsNoTracking().Where(a => a.ApplicationUserId == userId && a.IsPrimaryAddress == true).FirstOrDefault()?.Name;
        }

        public bool IsNameAlreadyInUse(string userId, string addressName, string originalAddressName)
        {
            if (originalAddressName is null)
            {
                return this.addressRepository.AllAsNoTracking().Any(a => a.Name == addressName && a.ApplicationUserId == userId);
            }
            else if (originalAddressName != addressName)
            {
                return this.addressRepository.AllAsNoTracking().Any(a => a.Name == addressName && a.ApplicationUserId == userId);
            }

            return false;
        }

        public IEnumerable<string> GetAddressNamesByUserId(string userId)
        {
            return this.addressRepository.AllAsNoTracking().Where(a => a.ApplicationUserId == userId).Select(a => a.Name).ToList();
        }

        public IEnumerable<string> GetAllowedDistricts()
        {
            return this.allowedDistricts;
        }

        public async Task<bool> DeleteAsync(string addressName)
        {
            var address = this.addressRepository.AllAsNoTracking().FirstOrDefault(a => a.Name == addressName);

            if (address is null)
            {
                return false;
            }

            var userId = address.ApplicationUserId;
            this.addressRepository.Delete(address);

            await this.addressRepository.SaveChangesAsync();

            if (address.IsPrimaryAddress && this.addressRepository.AllAsNoTracking().Any(a => a.ApplicationUserId == userId))
            {
                var newPrimary = this.addressRepository.All().FirstOrDefault(a => a.ApplicationUserId == userId);
                newPrimary.IsPrimaryAddress = true;
                await this.addressRepository.SaveChangesAsync();
            }

            return true;
        }

        public async Task CreateNewAddressAsync(AddAddressModel model, string userId)
        {
            var address = AutoMapperConfig.MapperInstance.Map<Address>(model);
            address.ApplicationUserId = userId;

            if (!this.addressRepository.AllAsNoTracking().Any(a => a.ApplicationUserId == userId))
            {
                address.IsPrimaryAddress = true;
            }
            else if (model.IsPrimaryAddress)
            {
                this.RemovePreviousPrimaryAddress(userId);
                address.IsPrimaryAddress = model.IsPrimaryAddress;
            }

            await this.addressRepository.AddAsync(address);
            await this.addressRepository.SaveChangesAsync();
        }

        public async Task UpdateAddressAsync(AddAddressModel model, string userId, string addressName)
        {
            var address = this.addressRepository.All().FirstOrDefault(a => a.ApplicationUserId == userId && a.Name == addressName);

            if (address is null)
            {
                throw new NullReferenceException();
            }

            address.Name = model.Name;
            address.Street = model.Street;
            address.AddressLineTwo = model.AddressLineTwo;
            address.District = model.District;
            address.City = model.City;
            address.Country = model.Country;
            address.PostCode = model.PostCode;

            if (model.IsPrimaryAddress)
            {
                this.RemovePreviousPrimaryAddress(userId);
                address.IsPrimaryAddress = model.IsPrimaryAddress;
            }

            await this.addressRepository.SaveChangesAsync();
        }

        private void RemovePreviousPrimaryAddress(string userId)
        {
            var previousPrimary = this.addressRepository.All().FirstOrDefault(a => a.IsPrimaryAddress && a.ApplicationUserId == userId);

            previousPrimary.IsPrimaryAddress = false;
        }
    }
}
