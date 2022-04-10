namespace Restaurant.Web.Infrastructure.Filters
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class AddModelErrorsToTempDataActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid && context.ModelState.Keys.Contains("Tempdata error"))
            {
                var modelErrors = new List<string>();

                foreach (var modelState in context.ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }

                modelErrors.RemoveAll(x => string.IsNullOrEmpty(x));
                var controller = context.Controller as Controller;
                controller.TempData.Add("ValidationErrors", modelErrors);
            }

            base.OnActionExecuted(context);
        }
    }
}
