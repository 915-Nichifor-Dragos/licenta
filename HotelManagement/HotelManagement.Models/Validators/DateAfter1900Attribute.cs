using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models.Validators;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DateAfter1900Attribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not DateTime date)
        {
            return false;
        }

        return date.Year > 1900 && date.Year < DateTime.UtcNow.Year + 1;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be after the year 1900";
    }
}
