namespace Restaurant.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Restaurant.Services.Data;
    using Restaurant.Services.Models;
    using Restaurant.Web.Infrastructure.ExtentionClasses;
    using Restaurant.Web.Infrastructure.Filters;
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
        [AddRouteIdActionFilter]

        public IActionResult AllAddresses(string addressName)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var model = this.addressService.GetByNameAndUserId<AddressViewModel>(userId, addressName);
            if (model is not null)
            {
                model.AllowedDistricts = this.addressService.GetAllowedDistricts();
            }

            return this.View(model);
        }

        [Authorize]
        [HttpPost("/Address/All/{addressName?}")]
        [AddRouteIdActionFilter]
        [UniqueAddressNameValidationActionFilter]
        public async Task<IActionResult> AllAddresses(string addressName, AddressViewModel model)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!this.ModelState.IsValid)
            {
                model.AddressNames = this.addressService.GetAddressNamesByUserId(userId);
                model.Name = addressName;
                model.AllowedDistricts = this.addressService.GetAllowedDistricts();
                return this.View(model);
            }

            var addAddressModel = new AddAddressModel
            {
                Name = model.Name,
                Street = model.Street,
                AddressLineTwo = model.AddressLineTwo,
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
        [HttpGet("/Address/Add")]

        public IActionResult AddAddress()
        {
            var model = new AddressViewModel
            {
                AllowedDistricts = this.addressService.GetAllowedDistricts(),
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost("/Address/Add")]
        [UniqueAddressNameValidationActionFilter]
        public async Task<IActionResult> AddAddress(AddressViewModel model)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!this.ModelState.IsValid)
            {
                model.AllowedDistricts = this.addressService.GetAllowedDistricts();
                return this.View(model);
            }

            var addAddressModel = new AddAddressModel
            {
                Name = model.Name,
                Street = model.Street,
                AddressLineTwo = model.AddressLineTwo,
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
