using HotelManagement.Models.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using HotelManagement.BusinessLogic.Extensions;
using HotelManagement.DataAccess.Extensions;

var builder = WebApplication.CreateBuilder(args);

EmailOptions options = new();
builder.Configuration.GetSection(nameof(EmailOptions))
    .Bind(options);

builder.Services.AddControllersWithViews();
builder.Services.AddHotelManagementLogic();
builder.Services.AddHotelManagementRepository(builder.Configuration);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.Name = "LoginCookie";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
        options.SlidingExpiration = true;
        options.LoginPath = new PathString("/Authentication/Login");
        options.Cookie.IsEssential = true;
        options.AccessDeniedPath = new PathString("/Error/Unauthorized");
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200",
        builder => builder.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(nameof(EmailOptions)));
builder.Services.Configure<ProfilePictureOptions>(builder.Configuration.GetSection(nameof(ProfilePictureOptions)));
builder.Services.Configure<GoogleDriveOptions>(builder.Configuration.GetSection(GoogleDriveOptions.Options));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowLocalhost4200");

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Error/NotFound");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Login}/{id?}");

app.Run();