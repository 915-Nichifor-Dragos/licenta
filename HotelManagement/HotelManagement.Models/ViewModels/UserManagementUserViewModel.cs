namespace HotelManagement.Models.ViewModels;

public class UserManagementUserViewModel
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
    public string Role { get; set; }
    public string Email { get; set; }

    public DateOnly BirthDate { get; set; }

    public DateOnly RegistrationDate { get; set; }
}
