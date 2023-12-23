using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models.Validators;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FileSizeAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return true;

        if (value is IFormFile file)
        {
            return file.Length < 1024 * 1024 * 10;
        }

        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"Size too big!";
    }
}
