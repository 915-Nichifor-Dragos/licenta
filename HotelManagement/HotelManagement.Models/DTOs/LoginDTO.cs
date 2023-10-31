using HotelManagement.Models.Constants;

namespace HotelManagement.Models.DTOs;

public class LoginDTO
{
    public string Username { get; set; }

    public string Email { get; set; }

    public Role Role { get; set; }
}
