using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models.ViewModels;

public class LoginViewModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public IEnumerable<ValidationResult> Validate()
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(this);

        Validator.TryValidateObject(this, context, validationResults, validateAllProperties: true);

        return validationResults;
    }
}
