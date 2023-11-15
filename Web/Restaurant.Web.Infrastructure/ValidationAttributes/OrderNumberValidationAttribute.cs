namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AdminDashboard.Data.Models;
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

                IdentityUser user;

                if (userManager is not null)
                {
                    user = userManager.GetUserAsync(httpContextAccessor.HttpContext.User).GetAwaiter().GetResult();
                }
                else
                {
                    var adminUserManager = validationContext.GetService<UserManager<AdminDashboardUser>>();
                    user = adminUserManager.GetUserAsync(httpContextAccessor.HttpContext.User).GetAwaiter().GetResult();
                }

                var orderService = validationContext.GetService<IOrderService>();
                IEnumerable<string> orderNumbers;

                if (user is AdminDashboardUser)
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
