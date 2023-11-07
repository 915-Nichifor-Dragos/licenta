using HotelManagement.BusinessLogic.Converters;
using HotelManagement.BusinessLogic.ILogic;
using HotelManagement.DataAccess.IRepository;
using HotelManagement.Models.Constants;
using HotelManagement.Models.DataModels;
using HotelManagement.Models.Options;
using HotelManagement.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace HotelManagement.BusinessLogic.Logic;

public class UserLogic : IUserLogic
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailLogic;
    private readonly IHotelLogic _hotelLogic;
    private readonly IFileStorageService _fileStorageService;
    private readonly GoogleDriveOptions _driveOptions;
    private readonly ProfilePictureOptions _profilePictureOptions;

    public UserLogic(IUserRepository userRepository, IEmailService emailLogic, IFileStorageService fileStorageService, IOptions<GoogleDriveOptions> configuration, IOptions<ProfilePictureOptions> options, IHotelLogic hotelLogic)
    {
        _userRepository = userRepository;
        _emailLogic = emailLogic;
        _fileStorageService = fileStorageService;
        _driveOptions = configuration.Value;
        _profilePictureOptions = options.Value;
        _hotelLogic = hotelLogic;
    }

    public UserValidityOutcomes CheckValidity(User user)
    {
        if (_userRepository.Any(u => u.Username == user.Username))
        {
            return UserValidityOutcomes.InvalidUsername;
        }

        if (_userRepository.Any(u => u.Email == user.Email))
        {
            return UserValidityOutcomes.InvalidEmail;
        }

        return UserValidityOutcomes.Valid;
    }

    public async Task<User?> GetUserById(Guid id)
    {
        return await _userRepository.GetById(id);
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await _userRepository.GetByUsername(username);
    }

    public async Task<User?> GetUserByUsernameWithHotels(string username)
    {
        return await _userRepository.GetByUsernameWithHotels(username);
    }

    public async Task CreateUser(User user, string host, int port)
    {
        if (user.ImageUrl == null)
        {
            user.ImageUrl = (string)_profilePictureOptions.GetType().GetProperty(Enum.GetName(typeof(Models.Constants.Role), user.RoleId - 1)).GetValue(_profilePictureOptions, null);
        }

        _userRepository.Add(user);

        await _emailLogic.ComposeConfirmationEmail(user, host, port);
        await _userRepository.SaveChanges();
    }

    public void UpdateUser(User user)
    {
        _userRepository.Update(user);
    }

    public async Task<bool> DeleteUser(Guid id, string authenticatedUsername)
    {
        var authenticatedUser = await GetUserByUsernameWithHotels(authenticatedUsername);

        if (authenticatedUser == null)
        {
            return false;
        }

        var hotelIds = authenticatedUser.UserHotels
            .Select(uh => uh.HotelsId)
            .ToList();

        var hotels = await _hotelLogic.GetByListId(hotelIds);

        var subordinates = hotels
            .SelectMany(uh => uh.UserHotels)
            .Select(uh => uh.UsersId)
            .ToList();

        if (authenticatedUser.Role.Name.Equals(Models.Constants.Role.Manager) && !subordinates.Contains(id))
        {
            return false;
        }

        await _userRepository.Delete(id);

        return true;
    }

    public async Task<UserValidityOutcomes> CheckEmailUniqueness(string email)
    {
        if (await _userRepository.AnyAsync(u => u.Email.Equals(email)))
        {
            return UserValidityOutcomes.InvalidEmail;
        }

        return UserValidityOutcomes.Valid;
    }

    public string CheckAge(string birthDate)
    {
        try
        {
            var dateOfBirth = DateOnly.Parse(birthDate);
            var maximumAcceptedDateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddYears(-18));

            return (dateOfBirth < maximumAcceptedDateOfBirth).ToString();
        }
        catch (Exception ex)
        {
            return "Format is incorrect!";
        }
    }

    public async Task<bool> IsValidLogin(LoginViewModel loginViewModel)
    {
        var hashedPassword = PasswordEncrypter.Hash(loginViewModel.Password);

        return await _userRepository.AnyAsync(u => u.Username == loginViewModel.Username && u.Password == hashedPassword);
    }

    public async Task<EmailValidityOutcomes> ActivateAccount(string email, string activationToken)
    {
        var user = await _userRepository.GetByEmail(email);

        if (user == null)
        {
            return EmailValidityOutcomes.InvalidUser;
        }

        if (user.ActivationToken != activationToken)
        {
            return EmailValidityOutcomes.InvalidToken;
        }

        TimeSpan timeDifference = (TimeSpan)(DateTime.UtcNow - user.TokenGenerationTime);
        double hoursDifference = timeDifference.TotalHours;

        if (hoursDifference >= 24)
        {
            return EmailValidityOutcomes.ExpiredToken;
        }

        await _userRepository.Activate(user.Id);

        return EmailValidityOutcomes.Valid;
    }

    //public async Task EditRole(EditUserViewModel editUserViewModel)
    //{
    //    var user = await GetUserByUsername(editUserViewModel.Username);

    //    if (user == null)
    //    {
    //        throw new ArgumentNullException(nameof(user), "user cannot be null.");
    //    }

    //    user.RoleId = editUserViewModel.Role;

    //    _userRepository.Update(user);

    //    await _userRepository.SaveChanges();
    //}

    public async Task<bool> UpdateUserInfo(string username, UserUpdateViewModel newUserDetails, string host, int port)
    {
        var user = await _userRepository.GetByUsername(username);
        var sendMail = false;

        if (user == null)
        {
            return false;
        }

        if (!user.Email.Equals(newUserDetails.Email))
        {
            sendMail = true;
        }

        user.Email = newUserDetails.Email;
        user.Bio = newUserDetails.Bio;
        user.BirthDate = newUserDetails.BirthDate.ToDateTime(new TimeOnly(10, 00));
        user.Gender = newUserDetails.Gender;
        user.Address = newUserDetails.Address;

        _userRepository.Update(user);

        if (sendMail)
        {
            await _emailLogic.ComposeConfirmationEmail(user, host, port);
            user.IsActive = false;
        }

        await _userRepository.SaveChanges();

        return sendMail;
    }

    public async Task AddHotelToUser(Hotel hotel, User user)
    {
        await _userRepository.AddHotelToUser(hotel, user);
    }

    public async Task<DateTime> GetRegistrationDate(Guid hotelId, Guid userId)
    {
        return await _userRepository.GetRegistrationDate(hotelId, userId);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _userRepository.GetByEmail(email);
    }

    public async Task DeleteProfilePicture(string name)
    {
        var user = await _userRepository.GetByUsername(name);

        if (user == null)
        {
            throw new Exception("The user doesn't exit!");
        }

        user.ImageUrl = (string)_profilePictureOptions.GetType().GetProperty(Enum.GetName(typeof(Models.Constants.Role), user.Role.Name)).GetValue(_profilePictureOptions, null);

        _userRepository.Update(user);
    }

    public async Task<bool> ResendActivationToken(string email, string host, int port)
    {
        var user = await _userRepository.GetByEmail(email);

        if (user == null)
        {
            return false;
        }

        await _emailLogic.ComposeConfirmationEmail(user, host, port);
        await _userRepository.SaveChanges();

        return true;
    }

    public async Task<string> UploadProfilePicture(IFormFile profilePicture)
    {
        if (profilePicture != null)
        {
            try
            {
                var imageUrl = await _fileStorageService.UploadImage(profilePicture.OpenReadStream());
                return imageUrl.Url;
            }
            catch (Exception ex)
            {
                return _driveOptions.DefaultProfilePicture;
            }
        }

        return _driveOptions.DefaultProfilePicture;
    }

    public async Task<PaginatedList<UserManagementUserViewModel>> GetByHotelId(
        Guid hotelId,
        UserListingSortType sortAttribute, 
        bool isAscending, 
        bool isOwner, 
        int pageSize, 
        int pageIndex)
    {
        var tupleItemsUsersAndCount = await _userRepository.GetByHotelId(
            hotelId,
            sortAttribute, 
            isAscending,
            isOwner, 
            pageSize, 
            pageIndex);

        var usersToSendInView = tupleItemsUsersAndCount.Item1
            .Select(u => UserConverter.FromUserToUserManagementUserViewModel(u, u.UserHotels.First().RegistrationDate))
            .ToList();
        var count = tupleItemsUsersAndCount.Item2;

        return new PaginatedList<UserManagementUserViewModel>(
            usersToSendInView,
            count, 
            pageIndex,
            pageSize,
            sortAttribute.ToString(),
            isAscending);
    }

    public async Task<PaginatedList<UserManagementUserViewModel>> GetAll(
        UserListingSortType sortAttribute, 
        bool isAscending, User user, 
        int pageSize, 
        int pageIndex)
    {
        var tupleItemsUsersAndCount = await _userRepository.GetAll(
            sortAttribute,
            isAscending,
            user, pageSize, 
            pageIndex);

        var usersToSendInView = tupleItemsUsersAndCount.Item1
            .Select(u => UserConverter.FromUserToUserManagementUserViewModel(u, u.UserHotels.First().RegistrationDate))
            .ToList();
        var count = tupleItemsUsersAndCount.Item2;

        return new PaginatedList<UserManagementUserViewModel>(
            usersToSendInView, 
            count, 
            pageIndex,
            pageSize,
            sortAttribute.ToString(), 
            isAscending);
    }

    public void UpdateUserRole(User user, int roleId)
    {
        user.RoleId = roleId;
        _userRepository.Update(user);
    }

    public async Task UpdateProfilePicture(string username, IFormFile profilePicture)
    {
        var user = await _userRepository.GetByUsername(username);

        if (user == null)
        {
            return;
        }

        var imageUrl = await UploadProfilePicture(profilePicture);

        user.ImageUrl = imageUrl;
        _userRepository.Update(user);
    }
}
