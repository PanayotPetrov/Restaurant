namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Restaurant.Data.Models;
    using Restaurant.Services.Data;

    public class OrderNumberValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string stringValue)
            {
                var httpContextAccessor = validationContext.GetService<IHttpContextAccessor>();

                var userManager = validationContext.GetService<UserManager<ApplicationUser>>();

                var user = userManager.GetUserAsync(httpContextAccessor.HttpContext.User).GetAwaiter().GetResult();

                var orderService = validationContext.GetService<IOrderService>();
                IEnumerable<string> orderNumbers;

                if (userManager.IsInRoleAsync(user, "Administrator").GetAwaiter().GetResult())
                {
                    orderNumbers = orderService.GetAllOrderNumbers();
                }
                else
                {
                    orderNumbers = orderService.GetAllOrderNumbersByUserId(user.Id);
                }

                if (orderNumbers.Contains(stringValue))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("Invalid order number provided!");
        }
    }
}
