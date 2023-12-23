using Microsoft.AspNetCore.Authentication.Cookies;

namespace HotelManagement.Web.Extensions;

public static class WebExtensions
{
    public static IServiceCollection AddCookies(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.LoginPath = "/Authentication/Login";
            options.AccessDeniedPath = "/Authentication/Register";
        });

        return services;
    }
}
