using HotelManagement.Models.DataModels;
using HotelManagement.Models.ViewModels;

namespace HotelManagement.BusinessLogic.Converters;

public class UserConverter
{
    public static UserDetailsViewModel FromUserToUserDetailsViewModel(User user)
    {
        return new UserDetailsViewModel()
        {
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            BirthDate = DateOnly.FromDateTime(user.BirthDate),
            Gender = user.Gender,
            Address = user.Address,
            Bio = user.Bio,
            Role = user.Role.Name,
        };
    }

    public static User FromUserUpdateViewModelToUser(UserUpdateViewModel user)
    {
        return new User
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Gender = user.Gender,
            Address = user.Address,
            Bio = user.Bio,
        };
    }

    public static User FromUserViewModelToUser(UserViewModel user, int roleId)
    {
        return new User
        {
            Username = user.Username,
            Password = PasswordEncrypter.Hash(user.Password),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            BirthDate = user.BirthDate.ToDateTime(TimeOnly.Parse("10:00 PM")),
            Gender = user.Gender,
            RoleId = roleId,
            Address = user.Address,
            Bio = user.Bio,
            ImageUrl = user.ImageUrl,
            IsActive = user.IsActive,
            ActivationToken = user.ActivationToken,
            TokenGenerationTime = user.TokenGenerationTime,
        };
    }
    
    public static User FromUserRegisterViewModelToUser(RegisterViewModel user, int roleId)
    {
        return new User
        {
            Username = user.Username,
            Password = PasswordEncrypter.Hash(user.Password),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            BirthDate = user.BirthDate,
            Gender = user.Gender,
            RoleId = roleId,
            Address = user.Address,
            Bio = user.Bio,

        };
    }
}
