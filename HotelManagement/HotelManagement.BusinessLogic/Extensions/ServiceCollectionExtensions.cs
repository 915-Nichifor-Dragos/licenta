using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using HotelManagement.BusinessLogic.ILogic;
using HotelManagement.BusinessLogic.Logic;
using Microsoft.Extensions.DependencyInjection;
using static Google.Apis.Drive.v3.DriveService;

namespace HotelManagement.BusinessLogic.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHotelManagementLogic(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFileStorageService, GoogleDriveImageUploaderService>();

        services.AddScoped<IUserLogic, UserLogic>();
        services.AddScoped<IRoleLogic, RoleLogic>();
        services.AddScoped<IHotelLogic, HotelLogic>();
        services.AddScoped<IRoomLogic, RoomLogic>();
        services.AddScoped<IBookingLogic, BookingLogic>();

        services.AddSingleton(new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = GoogleCredential.FromFile("credentials.json").CreateScoped(ScopeConstants.DriveFile),
            ApplicationName = "HMS Web Client"
        }));

        return services;
    }
}