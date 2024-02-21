namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CurrentDateValidationAttribute : ValidationAttribute
    {
        public CurrentDateValidationAttribute()
        {
            this.CurrentDate = DateTime.UtcNow;
            this.OneMonthAhead = DateTime.UtcNow.AddMonths(1);
        }

        public DateTime CurrentDate { get; }

        public DateTime OneMonthAhead { get; }

        public override bool IsValid(object value)
        {
            if (value is DateTime dateValue)
            {
                if (dateValue.Date >= this.CurrentDate.Date && dateValue.Date <= this.OneMonthAhead.Date)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
