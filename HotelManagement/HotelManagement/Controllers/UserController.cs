using HotelManagement.BusinessLogic.ILogic;
using HotelManagement.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Web.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController : ControllerBase{
    private readonly IUserLogic _userLogic;
    private readonly IRoleLogic _roleLogic;

    public UserController(IUserLogic userLogic, IRoleLogic roleLogic)
    {
        _userLogic = userLogic;
        _roleLogic = roleLogic;
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
                return Ok("Succesfully updated user information");
            }

            return BadRequest("Could not update user information");
        }

        return BadRequest("Something went wrong");
    }

    [HttpPut("profile-picture")]
    public async Task<IActionResult> UpdateProfilePicture(IFormFile ProfilePicture)
    {
        if (ModelState.IsValid)
        {
            await _userLogic.UpdateProfilePicture(User.Identity.Name, ProfilePicture);

            return Ok("Succesfully updated profile picture");
        }

        return BadRequest("Something went wrong");
    }

    [HttpDelete("profile-picture")]
    public async Task<IActionResult> DeleteProfilePicture()
    {
        try
        {
            await _userLogic.DeleteProfilePicture(User.Identity.Name);

            return Ok("Succesfully deleted profile picture");
        }
        catch (Exception)
        {
            return BadRequest("Could not delete picture");
        }
    }
}
