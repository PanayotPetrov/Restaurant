namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.Extensions.DependencyInjection;
    using Restaurant.Services.Data;

    public class ReservationIdValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string reservationId)
            {
                var reservationService = validationContext.GetService<IReservationService>();
                var result = reservationService.IsValid(reservationId);

                if (result)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("Invalid reservationId provided!");
        }
    }
}
