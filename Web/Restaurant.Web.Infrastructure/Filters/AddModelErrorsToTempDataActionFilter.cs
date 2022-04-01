namespace Restaurant.Web.Infrastructure.Filters
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class AddModelErrorsToTempDataActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
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

                var controller = context.Controller as Controller;
                controller.TempData.Add("ValidationErrors", modelErrors);
            }

            base.OnActionExecuted(context);
        }
    }
}
