using HotelManagement.Models.Constants;
using HotelManagement.Models.DataModels;
using HotelManagement.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace HotelManagement.BusinessLogic.ILogic;
public interface IUserLogic
{
    Task<User?> GetUserById(Guid id);
    Task<User?> GetUserByUsername(string username);
    Task<User?> GetUserByUsernameWithHotels(string username);
    Task<DateTime> GetRegistrationDate(Guid hotelId, Guid userId);
    Task<User> GetUserByEmail(string email);
    //Task<PaginatedList<UserDetailsViewModel>> GetByHotelId(Guid hotelId, UserListingSortType sortAttribute, bool isAscending, bool isOwner, int pageSize, int pageIndex);
    //Task<PaginatedList<UserDetailsViewModel>> GetAll(UserListingSortType sortAttribute, bool isAscending, User user, int pageSize, int pageIndex);
    public UserValidityOutcomes CheckValidity(User user);
    public Task<UserValidityOutcomes> CheckEmailUniqueness(string email);
    public string CheckAge(string birthDate);
    Task CreateUser(User user, string host, int port);
    void UpdateUser(User user);
    void UpdateUserRole(User user, int roleId);
    public Task<bool> UpdateUserInfo(string username, UserUpdateViewModel newUserDetails, string host, int port);
    //public Task EditRole(EditUserViewModel editUserViewModel);
    Task<bool> DeleteUser(Guid id, string authenticatedUsername);
    Task<bool> IsValidLogin(LoginViewModel loginViewModel);
    public Task<EmailValidityOutcomes> ActivateAccount(string email, string activationToken);
    Task<bool> ResendActivationToken(string email, string host, int port);
    Task AddHotelToUser(Hotel hotel, User user);
    public Task DeleteProfilePicture(string name);
    public Task<string> UploadProfilePicture(IFormFile profilePicture);
    Task UpdateProfilePicture(string username, IFormFile profilePicture);
}
