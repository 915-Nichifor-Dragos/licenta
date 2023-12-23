using HotelManagement.Models.Constants;

namespace HotelManagement.Models.ViewModels;

public class UserDetailsViewModel
{
    public string Username { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public DateOnly BirthDate { get; set; }

    public Gender Gender { get; set; }

    public string? Address { get; set; }

    public string? Bio { get; set; }

    public Role Role { get; set; }
}
