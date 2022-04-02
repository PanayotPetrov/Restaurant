namespace Restaurant.Web.Infrastructure.Filters
{
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using Restaurant.Services.Data;

    public class AddOrderRouteIdActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var orderService = context.HttpContext.RequestServices.GetService<IOrderService>();

            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var queryStringActionArguments = context.ActionArguments.Where(x => x.Key == "orderNumber").FirstOrDefault();

            if (queryStringActionArguments.Value is null)
            {
                var orderNumber = orderService.GetAllOrderNumbersByUserId(userId).FirstOrDefault();
                context.ActionArguments.Add("orderNumber", orderNumber);
            }

            base.OnActionExecuting(context);
        }
    }
}
