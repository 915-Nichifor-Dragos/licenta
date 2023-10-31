using HotelManagement.BusinessLogic.Converters;
using HotelManagement.BusinessLogic.ILogic;
using HotelManagement.Models.Constants;
using HotelManagement.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using HotelManagement.Models.DTOs;

namespace HotelManagement.Web.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserLogic _userLogic;

    public AuthenticationController(IUserLogic userLogic)
    {
        _userLogic = userLogic;
    }

    [HttpGet("logged-user-details")]
    public async Task<IActionResult> GetLoggedUserDetails()
    {
        var user = await _userLogic.GetUserByUsername(User.Identity.Name);

        if (user == null)
        {
            return BadRequest("Something went wrong");
        }

        return Ok(UserConverter.FromUserToUserDetailsViewModel(user));
    }

    [HttpGet("logged-user-role")]
    public async Task<IActionResult> GetLoggedUserRole()
    {
        var user = await _userLogic.GetUserByUsername(User.Identity.Name);

        if (user == null)
        {
            return Ok(new {});
        }

        return Ok(new { user.Role.Name });
    }

    [HttpGet("logged-user-username")]
    public async Task<IActionResult> GetLoggedUserUsername()
    {
        var user = await _userLogic.GetUserByUsername(User.Identity.Name);

        if (user == null)
        {
            return Ok(new {});
        }

        return Ok(new { user.Username });
    }

    [HttpPost("activate")]
    public async Task<IActionResult> Activate(ActivateViewModel model)
    {
        var emailValidityOutcomes = await _userLogic.ActivateAccount(model.Email, model.Token);

        return emailValidityOutcomes switch
        {
            EmailValidityOutcomes.Valid => Ok("Valid"),
            EmailValidityOutcomes.InvalidUser => Ok("Invalid user"),
            EmailValidityOutcomes.InvalidToken => Ok("Invalid token"),
            EmailValidityOutcomes.ExpiredToken => Ok("Expired token"),
            _ => BadRequest("Something went wrong..."),
        };
    }

    [HttpPost("resend-confirmation")]
    public async Task<IActionResult> ResendConfirmationEmail(string email)
    {
        var host = HttpContext.Request.Host.Host;
        var port = HttpContext.Request.Host.Port ?? 80;

        if (await _userLogic.ResendActivationToken(email, host, port))
        {
            return Ok($"An email was sent to {email}.");
        }

        return BadRequest("Something went wrong...");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserViewModel userViewModel)
    {
        IEnumerable<ValidationResult> validationResults = userViewModel.Validate();

        var user = UserConverter.FromUserViewModelToUser(userViewModel, 1);

        if (!validationResults.Any())
        {
            var validityOutcome = _userLogic.CheckValidity(user);

            if (validityOutcome == UserValidityOutcomes.InvalidEmail)
            {
                return BadRequest("Email is already in use");
            }

            if (validityOutcome == UserValidityOutcomes.InvalidUsername)
            {
                return BadRequest("Username is already in use");
            }

            user.ImageUrl = await _userLogic.UploadProfilePicture(userViewModel.ProfilePicture);

            string host = HttpContext.Request.Host.Host;
            int port = HttpContext.Request.Host.Port ?? 80;

            await _userLogic.CreateUser(user, host, port);

            return Ok("Your account was created");
        }

        return BadRequest("Your account was not validated. Please try again!");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        IEnumerable<ValidationResult> validationResults = loginViewModel.Validate();

        if (validationResults.Any())
        {
            return BadRequest("Invalid user entry");
        }

        if (!await _userLogic.IsValidLogin(loginViewModel))
        {
            return BadRequest("Username or password are incorrect");
        }

        var user = await _userLogic.GetUserByUsername(loginViewModel.Username);

        if (user == null)
        {
            return BadRequest("Something went wrong");
        }

        if (user.IsActive == false)
        {
            return BadRequest("Please activate your account");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.Name.ToString()),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var authProperties = new AuthenticationProperties
        {
            IssuedUtc = DateTimeOffset.UtcNow,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(120),
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

        Response.Cookies.Append("hotel-management", "authentication-cookie", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddMinutes(120)
        });

        return Ok(new LoginDTO 
        { 
            Username = user.Username,
            Role = user.Role.Name,
        } );
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
               CookieAuthenticationDefaults.AuthenticationScheme);

        Response.Cookies.Append("hotel-management", "authentication-expired", new CookieOptions
        {
            Expires = DateTime.Now.AddDays(-1),
            IsEssential = true
        });


        return Ok();
    }
}
