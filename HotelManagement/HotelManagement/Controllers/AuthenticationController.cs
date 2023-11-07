using HotelManagement.BusinessLogic.Converters;
using HotelManagement.BusinessLogic.ILogic;
using HotelManagement.Models.Constants;
using HotelManagement.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

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

    [HttpGet("logged-user-claims")]
    public IActionResult GetLoggedUserClaims()
    {
        var claims = User.Claims;

        var usernameClaim = claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        var roleClaim = claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(usernameClaim) || string.IsNullOrEmpty(roleClaim))
        {
            return Ok(new
            {
                Username = "",
                Role = ""
            });
        }

        return Ok(new
        {
            Username = usernameClaim,
            Role = roleClaim
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
    {
        IEnumerable<ValidationResult> validationResults = loginViewModel.Validate();

        if (validationResults.Any())
        {
            return BadRequest(new
            {
                Success = false,
                Message = "Invalid model: username or password missing"
            });
        }

        if (!await _userLogic.IsValidLogin(loginViewModel))
        {
            return BadRequest(new
            {
                Success = false,
                Message = "Username or password are incorrect"
            });
        }

        var user = await _userLogic.GetUserByUsername(loginViewModel.Username);

        if (user == null)
        {
            return BadRequest(new 
            { 
                Success = false, 
                Message = "Something went wrong" 
            });
        }

        if (user.IsActive == false)
        {
            return BadRequest(new
            {
                Success = false,
                Message = "Please activate your account"
            });
        }

        var claims = new List<Claim>
        {
            new Claim(type: ClaimTypes.Name, value: user.Username),
            new Claim(type: ClaimTypes.Role, value: user.Role.Name.ToString()),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(identity),
            new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(120)
            });

        return Ok(new
        {
            Success = true,
            Message = "Signed in successfully"
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
               CookieAuthenticationDefaults.AuthenticationScheme);

        return Ok();
    }

    [HttpPost("activate")] // to be tested
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

    [HttpPost("resend-confirmation")] // to be tested
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
    public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
    {
        IEnumerable<ValidationResult> validationResults = registerViewModel.Validate();

        var user = UserConverter.FromUserRegisterViewModelToUser(registerViewModel, 1);

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

            // user.ImageUrl = await _userLogic.UploadProfilePicture(registerViewModel.ProfilePicture);

            string host = HttpContext.Request.Host.Host;
            int port = HttpContext.Request.Host.Port ?? 80;

            await _userLogic.CreateUser(user, host, port);

            return Ok();
        }

        return BadRequest("Your account was not validated. Please try again!");
    }
}
