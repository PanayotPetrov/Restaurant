namespace Restaurant.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Restaurant.Services.Data;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;
    using Restaurant.Web.Infrastructure.Filters;
    using Restaurant.Web.Infrastructure.ValidationAttributes;
    using Restaurant.Web.ViewModels.Address;

    [Authorize]
    public class AddressController : BaseController
    {
        private readonly IAddressService addressService;

        public AddressController(IAddressService addressService)
        {
            this.addressService = addressService;
        }

        [HttpGet("/Address/All/{addressName?}")]
        [AddAddressRouteIdActionFilter]

        public IActionResult AllAddresses([AddressNameValidation]string addressName)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.UrlNotFound));
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var model = this.addressService.GetByUserIdAndAddressName<AddressViewModel>(userId, addressName);
            if (model is not null)
            {
                model.AllowedDistricts = this.addressService.GetAllowedDistricts();
            }

            return this.View(model);
        }

        [HttpPost("/Address/All/{addressName?}")]
        [AddAddressRouteIdActionFilter]
        [UniqueAddressNameValidationActionFilter]
        public async Task<IActionResult> AllAddresses(string addressName, AddressViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.AddressNames = this.addressService.GetAddressNamesByUserId(model.ApplicationUserId);
                model.Name = addressName;
                model.AllowedDistricts = this.addressService.GetAllowedDistricts();
                return this.View(model);
            }

            var addAddressModel = AutoMapperConfig.MapperInstance.Map<AddAddressModel>(model);

            await this.addressService.UpdateAddressAsync(addAddressModel, model.ApplicationUserId, addressName);
            return this.RedirectToAction(nameof(this.AllAddresses), new { addressName = model.Name });
        }

        [HttpGet("/Address/Add")]

        public IActionResult AddAddress()
        {
            var model = new AddressViewModel
            {
                AllowedDistricts = this.addressService.GetAllowedDistricts(),
            };

            return this.View(model);
        }

        [HttpPost("/Address/Add")]
        [UniqueAddressNameValidationActionFilter]
        public async Task<IActionResult> AddAddress(AddressViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.AllowedDistricts = this.addressService.GetAllowedDistricts();
                return this.View(model);
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var addAddressModel = AutoMapperConfig.MapperInstance.Map<AddAddressModel>(model);

            await this.addressService.CreateNewAddressAsync(addAddressModel, userId);
            return this.Redirect($"/Address/All/{model.Name}");
        }

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
