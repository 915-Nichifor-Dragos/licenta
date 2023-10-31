using HotelManagement.DataAccess.Contexts;
using HotelManagement.Models.Constants;
using HotelManagement.Models.DataModels;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.DataAccess.Seeders;

public static class RoleSeeder
{
    public static async Task SeedRoles(HotelManagementContext context)
    {
        var roles = Enum.GetValues(typeof(Models.Constants.Role)).Cast<Models.Constants.Role>();
        var existingRoles = await context.Roles.ToListAsync();

        foreach (var role in roles)
        {
            var roleName = role;
            var existingRole = existingRoles.FirstOrDefault(r => r.Name == roleName);

            if (existingRole == null)
            {
                context.Roles.Add(new Models.DataModels.DBRole { Name = roleName });
            }
        }

        await context.SaveChangesAsync();
    }
}
