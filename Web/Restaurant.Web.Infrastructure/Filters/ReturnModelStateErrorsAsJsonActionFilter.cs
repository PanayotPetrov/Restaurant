namespace Restaurant.Web.Infrastructure.Filters
{
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ReturnModelStateErrorsAsJsonActionFilter : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var modelErrors = new List<string>();

                foreach (var modelState in context.ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }

                context.HttpContext.Response.StatusCode = 400;
                context.HttpContext.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(modelErrors);
                await context.HttpContext.Response.WriteAsync(json);
                context.HttpContext.Response.Body.Flush();
                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
