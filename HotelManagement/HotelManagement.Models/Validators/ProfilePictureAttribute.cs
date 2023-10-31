using FileSignatures;
using FileSignatures.Formats;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models.Validators;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ProfilePictureAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return true;

        if (value is IFormFile file)
        {
            var inspector = new FileFormatInspector();
            var format = inspector.DetermineFileFormat(file.OpenReadStream());

            if (format is not Image)
            {
                return false;
            }
            return true;
        }

        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"Wrong format for the picture.";
    }
}
