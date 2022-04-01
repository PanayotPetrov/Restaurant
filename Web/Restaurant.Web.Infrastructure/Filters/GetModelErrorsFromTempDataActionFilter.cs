namespace Restaurant.Web.Infrastructure.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class GetModelErrorsFromTempDataActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;

            if (controller.TempData.ContainsKey("ValidationErrors"))
            {
                string[] errors = controller.TempData["ValidationErrors"] as string[];

                foreach (var error in errors)
                {
                    context.ModelState.AddModelError(string.Empty, error);
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
