using HotelManagement.BusinessLogic.Converters;
using HotelManagement.BusinessLogic.ILogic;
using HotelManagement.Models.Constants;
using HotelManagement.Models.DataModels;
using HotelManagement.Models.ViewModels;
using HotelManagement.Web.Authorize;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace HotelManagement.Web.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController : ControllerBase{
    private readonly IUserLogic _userLogic;
    private readonly IRoleLogic _roleLogic;
    private readonly IHotelLogic _hotelLogic;

    public UserController(IUserLogic userLogic, IRoleLogic roleLogic, IHotelLogic hotelLogic)
    {
        _userLogic = userLogic;
        _roleLogic = roleLogic;
        _hotelLogic = hotelLogic;
    }

    [HttpGet("subordinates")]
    [AuthorizeRoles(Role.Manager, Role.Owner)]
    public async Task<ActionResult> GetAllSubordinates(
        UserListingSortType sortAttribute, 
        bool isAscending, 
        int? pageSize, 
        int? pageIndex)
    {
        var authenticatedUsername = User.FindFirst(ClaimTypes.Name).Value;
        var authenticatedUser = await _userLogic.GetUserByUsernameWithHotels(authenticatedUsername);

        if (authenticatedUser == null)
        {
            return BadRequest();
        }

        var paginatedList = await _userLogic.GetAll(
            sortAttribute, 
            isAscending, 
            authenticatedUser, 
            pageSize ?? 5, 
            pageIndex ?? 1);

        var result = new UserManagementPaginator
        {
            Users = paginatedList.Item1,
            Count = paginatedList.Item2
        };

        return Ok(result);
    }

    [HttpGet("subordinates/{hotelId}")]
    [AuthorizeRoles(Role.Manager, Role.Owner)]
    public async Task<ActionResult> GetSubordinatesByHotelId(
        Guid hotelId,
        UserListingSortType sortAttribute, 
        bool isAscending,
        int? pageSize,
        int? pageIndex)
    {
        var hotel = await _hotelLogic.GetById(hotelId);

        if (hotel != null)
        {
            var paginatedList = await _userLogic.GetByHotelId(
                hotelId,
                sortAttribute,
                isAscending, 
                User.IsInRole(Role.Owner.ToString()), 
                pageSize ?? 5,
                pageIndex ?? 1);

            var result = new UserManagementPaginator
            {
                Users = paginatedList.Item1,
                Count = paginatedList.Item2
            };

            return Ok(result);
        }

        return BadRequest();
    }

    [HttpGet("role/{userId}")]
    [AuthorizeRoles(Role.Manager, Role.Owner)]
    public async Task<IActionResult> GetUsernameAndRoleById(string userId)
    {
        var user = await _userLogic.GetUserById(Guid.Parse(userId));

        if (user == null)
        {
            return BadRequest();
        }

        var result = new UserRoleEditViewModel {
            Username = user.Username,
            Role = user.Role.Name
        };

        return Ok(result);
    }

    [AuthorizeRoles(Role.Manager, Role.Owner)]
    [HttpPut("role")]
    public async Task<IActionResult> UpdateRole(string username, Role changedRole)
    {
        var user = await _userLogic.GetUserByUsername(username);

        if (user == null)
        {
            return BadRequest();
        }

        DBRole newRole = await _roleLogic.GetRoleByName(changedRole);

        _userLogic.UpdateUserRole(user, newRole.Id);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUserInformation(UserUpdateViewModel model)
    {
        IEnumerable<ValidationResult> validationResults = model.Validate();

        if (!validationResults.Any())
        {
            var host = HttpContext.Request.Host.Host;
            var port = HttpContext.Request.Host.Port ?? 80;

            if (await _userLogic.UpdateUserInfo(User.Identity.Name, model, host, port))
            {
                return Ok();
            }

            return BadRequest();
        }

        return BadRequest();
    }

    [HttpPut("profile-picture")]
    public async Task<IActionResult> UpdateProfilePicture(IFormFile ProfilePicture)
    {
        if (ModelState.IsValid)
        {
            await _userLogic.UpdateProfilePicture(User.Identity.Name, ProfilePicture);

            return Ok();
        }

        return BadRequest();
    }

    [HttpDelete("profile-picture")]
    public async Task<IActionResult> DeleteProfilePicture()
    {
        try
        {
            await _userLogic.DeleteProfilePicture(User.Identity.Name);

            return Ok();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    [AuthorizeRoles(Role.Owner, Role.Manager)]
    public async Task<IActionResult> AddUser(UserAddViewModel addUserViewModel)
    {
        var validationResults = addUserViewModel.Validate();

        var user = UserConverter.FromUserAddViewModelToUser(addUserViewModel);

        if (!validationResults.Any())
        {
            var validityOutcome = _userLogic.CheckValidity(user);

            if (validityOutcome == UserValidityOutcomes.InvalidEmail)
            {
                return BadRequest("Invalid email");
            }

            if (validityOutcome == UserValidityOutcomes.InvalidUsername)
            {
                return BadRequest("Invalid username");
            }

            var host = HttpContext.Request.Host.Host;
            var port = HttpContext.Request.Host.Port ?? 80;

            await _userLogic.CreateUser(user, host, port);

            var hotel = await _hotelLogic.GetById(addUserViewModel.HotelId);

            await _hotelLogic.AddUserToHotel(hotel, user);
        }

        return Ok();
    }

    [HttpDelete]
    [AuthorizeRoles(Role.Manager, Role.Owner)]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var authenticatedUsername = User.FindFirst(ClaimTypes.Name).Value;

        var success = await _userLogic.DeleteUser(Guid.Parse(id), authenticatedUsername);

        return Ok(new { success });
    }
}
