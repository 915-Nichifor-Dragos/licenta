using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using HotelManagement.DataAccess.Contexts;
using HotelManagement.DataAccess.Seeders;

namespace HotelManagement.DataAccess.ContextFactories;
public class HotelManagementContextFactory : IDesignTimeDbContextFactory<HotelManagementContext>
{
    public HotelManagementContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<HotelManagementContext>();
        optionsBuilder.UseSqlServer(args[0]);
        HotelManagementContext hotelManagementContext = new(optionsBuilder.Options);

        SeedRolesAsync(hotelManagementContext).Wait();

        return hotelManagementContext;
    }

    private async Task SeedRolesAsync(HotelManagementContext context)
    {
        await RoleSeeder.SeedRoles(context);
    }
}
