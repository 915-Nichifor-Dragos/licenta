using HotelManagement.Models.DataModels;
using HotelManagement.Models.ViewModels;

namespace HotelManagement.BusinessLogic.Converters;

public class UserConverter
{
    public static UserManagementUserViewModel FromUserToUserManagementUserViewModel(User user, DateTime registrationDate)
    {
        return new UserManagementUserViewModel()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.Name.ToString(),
            Email = user.Email,
            BirthDate = DateOnly.FromDateTime(user.BirthDate),
            RegistrationDate = DateOnly.FromDateTime(registrationDate)
        };
    }

    public static User FromUserUpdateViewModelToUser(UserUpdateViewModel user)
    {
        return new User
        {
            Id = user.Id,
            FirstName = user.FirstName,
            Email = user.Email,
            Gender = user.Gender,
            Address = user.Address,
            Bio = user.Bio,
            LastName = user.LastName,
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

    public static User FromUserAddViewModelToUser(UserAddViewModel userAddViewModel)
    {
        if (userAddViewModel == null)
        {
            return null;
        }

        return new User
        {
            Username = userAddViewModel.Username,
            Password = PasswordEncrypter.Hash(userAddViewModel.Password),
            FirstName = userAddViewModel.FirstName,
            LastName = userAddViewModel.LastName,
            Email = userAddViewModel.Email,
            BirthDate = userAddViewModel.BirthDate.ToDateTime(TimeOnly.Parse("10:00 PM")),
            Gender = userAddViewModel.Gender,
            RoleId = userAddViewModel.RoleId,
            IsDeleted = false
        };
    }
}
