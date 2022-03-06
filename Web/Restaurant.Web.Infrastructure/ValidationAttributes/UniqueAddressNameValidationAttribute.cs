namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    public class UniqueAddressNameValidationAttribute : ValidationAttribute
    {
        // public override bool RequiresValidationContext => true;

        // protected override ValidationResult IsValid(object value, ValidationContext context)
        // {
        //    var addressService = context.GetService(typeof(IAddressService));

        // }
    }
}
