namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class ValidateUniqueAddressNameAttribute : Attribute
    {
    }
}
