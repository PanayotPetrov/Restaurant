namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Restaurant.Services.Data;

    public class OrderNumberValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string stringValue)
            {
                var httpContextAccessor = validationContext.GetService<IHttpContextAccessor>();
                var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var orderService = validationContext.GetService<IOrderService>();
                var orderNumbers = orderService.GetAllOrderNumbersByUserId(userId);

                if (orderNumbers.Contains(stringValue))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("Invalid order number provided!");
        }
    }
}
