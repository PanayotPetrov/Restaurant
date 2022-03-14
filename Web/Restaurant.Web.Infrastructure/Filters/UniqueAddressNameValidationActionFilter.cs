namespace Restaurant.Web.Infrastructure.Filters
{
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using Restaurant.Services.Data;
    using Restaurant.Web.Infrastructure.ValidationAttributes;

    public class UniqueAddressNameValidationActionFilter : ActionFilterAttribute
    {
        public UniqueAddressNameValidationActionFilter()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelActionArguments = context.ActionArguments.Where(x => x.Key == "model").FirstOrDefault();

            var routeIdActionArguments = context.ActionArguments.Where(x => x.Key == "addressName").FirstOrDefault();

            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var propertyToValidate = modelActionArguments.Value
                .GetType()
                .GetProperties()
                .Where(x => x.GetCustomAttributes(true).Any(y => y.GetType() == typeof(ValidateUniqueAddressNameAttribute)))
                .FirstOrDefault();

            var newAddressName = propertyToValidate.GetValue(modelActionArguments.Value).ToString();

            var originalAddressName = routeIdActionArguments.Value?.ToString();

            var addressService = context.HttpContext.RequestServices.GetService<IAddressService>();

            if (!addressService.IsNameUnique(userId, newAddressName, originalAddressName))
            {
                context.ModelState.AddModelError(propertyToValidate.Name, "Address names must be unique.");
            }

            base.OnActionExecuting(context);
        }
    }
}
