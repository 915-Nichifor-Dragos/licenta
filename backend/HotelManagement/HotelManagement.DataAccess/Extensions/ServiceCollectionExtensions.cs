using EntityFrameworkCore.UseRowNumberForPaging;
using HotelManagement.DataAccess.Contexts;
using HotelManagement.DataAccess.IRepository;
using HotelManagement.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelManagement.DataAccess.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHotelManagementRepository(this IServiceCollection services, IConfiguration Configuration)
    {
        var connectionString = Configuration.GetConnectionString("HotelManagementDB");

        services.AddDbContext<HotelManagementContext>(options => options.UseSqlServer(connectionString, i => i.UseRowNumberForPaging()));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IHotelRepository, HotelRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        return services;
    }
}
