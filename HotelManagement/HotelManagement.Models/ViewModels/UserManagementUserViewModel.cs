namespace HotelManagement.Models.ViewModels;

public class UserManagementUserViewModel
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
    public string RoleName { get; set; }
    public string Email { get; set; }

    public DateOnly BirthDate { get; set; }

    public DateOnly RegistrationDate { get; set; }
}
