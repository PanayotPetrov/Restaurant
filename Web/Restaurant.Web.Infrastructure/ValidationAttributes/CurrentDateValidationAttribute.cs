namespace Restaurant.Web.Infrastructure.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;

    public class CurrentDateValidationAttribute : ValidationAttribute
    {
        public CurrentDateValidationAttribute()
        {
            this.CurrentDate = DateTime.Now;
            this.OneMonthAhead = DateTime.Now.AddMonths(1);
            this.ErrorMessage = $"Your reservation must be between {this.CurrentDate.ToShortDateString()} and {this.OneMonthAhead.ToShortDateString()}";
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
