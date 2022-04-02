namespace Restaurant.Web.Infrastructure.Filters
{
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;

    using Restaurant.Services.Data;

    public class AddAddressRouteIdActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var addressService = context.HttpContext.RequestServices.GetService<IAddressService>();

            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var queryStringActionArguments = context.ActionArguments.Where(x => x.Key == "addressName").FirstOrDefault();

            if (queryStringActionArguments.Value is null)
            {
                var primaryAddress = addressService.GetPrimaryAddressName(userId);
                context.ActionArguments.Add("addressName", primaryAddress);
            }

            base.OnActionExecuting(context);
        }
    }
}
