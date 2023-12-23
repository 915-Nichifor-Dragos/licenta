using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models.Validators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IsOver18Attribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is not DateOnly date)
            {
                return false;
            }

            return DateTime.UtcNow.AddYears(-18) >= date.ToDateTime(TimeOnly.MinValue);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Is not over 18 years old!";
        }
    }
}
