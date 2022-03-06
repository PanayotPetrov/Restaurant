namespace Restaurant.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Services.Models;
    using Restaurant.Web.ViewModels.Address;

    public class AddressController : BaseController
    {
        private readonly IAddressService addressService;

        public AddressController(IAddressService addressService)
        {
            this.addressService = addressService;
        }

        [Authorize]
        [HttpGet("/Address/All/{addressName?}")]
        public IActionResult AllAddresses(string addressName)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            AddressViewModel model;
            if (addressName is null)
            {
                model = this.addressService.GetPrimaryAddress<AddressViewModel>(userId);
            }
            else
            {
                model = this.addressService.GetByNameAndUserId<AddressViewModel>(userId, addressName);
            }

            return this.View(model);
        }

        [Authorize]
        [HttpPost("/Address/Add")]
        public async Task<IActionResult> AddAddress(AddressViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.AllAddresses));
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var addAddressModel = new AddAddressModel
            {
                Name = model.Name,
                Street = model.Street,
                District = model.District,
                City = model.City,
                PostCode = model.PostCode,
                Country = model.Country,
                IsPrimaryAddress = model.IsPrimaryAddress,
            };

            await this.addressService.CreateNewAddressAsync(addAddressModel, userId);
            return this.Redirect($"/Address/All/{model.Name}");
        }

        [Authorize]
        [HttpPost("/Address/All/{addressName?}")]
        public async Task<IActionResult> EditAddress(string addressName, AddressViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.AllAddresses));
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (addressName is null)
            {
                addressName = this.addressService.GetPrimaryAddress<AddressViewModel>(userId).Name;
            }

            var addAddressModel = new AddAddressModel
            {
                Name = model.Name,
                Street = model.Street,
                District = model.District,
                City = model.City,
                PostCode = model.PostCode,
                Country = model.Country,
                IsPrimaryAddress = model.IsPrimaryAddress,
            };

            await this.addressService.UpdateAddressAsync(addAddressModel, userId, addressName);
            return this.Redirect($"/Address/All/{model.Name}");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(AddressViewModel model)
        {
            try
            {
                string addressName = model.Name;

                await this.addressService.DeleteAsync(addressName);
                return this.RedirectToAction(nameof(this.AllAddresses));
            }
            catch (System.NullReferenceException)
            {
                this.ModelState.AddModelError(string.Empty, "You don't have any addresses to delete.");
                return this.Redirect($"/Address/All");
            }
        }
    }
}
