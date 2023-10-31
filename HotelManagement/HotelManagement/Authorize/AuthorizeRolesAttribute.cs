using HotelManagement.Models.Constants;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagement.Web.Authorize;

public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params Role[] roles)
    {
        var allowedRoles = roles.Select(r => Enum.GetName(typeof(Role), r));

        Roles = string.Join(",", allowedRoles);
    }
}