using HotelManagement.BusinessLogic.ILogic;
using HotelManagement.Models.Constants;
using HotelManagement.Models.ViewModels;
using HotelManagement.Web.Authorize;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelManagement.Web.Controllers;

[ApiController]
[Route("api/hotels")]
public class HotelController : ControllerBase
{
    private readonly IUserLogic _userLogic;
    private readonly IHotelLogic _hotelLogic;

    public HotelController(IUserLogic userLogic, IHotelLogic hotelLogic)
    {
        _userLogic = userLogic;
        _hotelLogic = hotelLogic;
    }

    [AuthorizeRoles(Role.Owner, Role.Manager)]
    [HttpGet("user-management-hotels")]
    public async Task<IActionResult> GetHotels(string? name)
    {
        var authenticatedUsername = User.FindFirst(ClaimTypes.Name).Value;
        var authenticatedUser = await _userLogic.GetUserByUsernameWithHotels(authenticatedUsername);

        if (authenticatedUser == null)
        {
            return BadRequest();
        }

        var hotels = await _hotelLogic.GetHotelsBySubstringAndCount(20, authenticatedUser, name);

        IEnumerable<UserManagementHotelViewModel> hotelData = hotels.Select(hotel => new UserManagementHotelViewModel
        {
            Id = hotel.Id,
            Name = hotel.Name
        });

        return Ok(hotelData);
    }
}