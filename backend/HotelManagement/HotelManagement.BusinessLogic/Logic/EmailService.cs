using HotelManagement.Models.DataModels;
using HotelManagement.BusinessLogic.ILogic;
using HotelManagement.DataAccess.IRepository;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Microsoft.Extensions.Options;
using HotelManagement.Models.Options;
using HotelManagement.BusinessLogic.Converters;

namespace HotelManagement.BusinessLogic.Logic;

public class EmailService : IEmailService
{
    private readonly IUserRepository _userRepository;

    private readonly EmailOptions _options;

    public EmailService(IUserRepository userRepository, IOptionsSnapshot<EmailOptions> options)
    {
        _userRepository = userRepository;
        _options = options.Value;
    }

    public async Task ComposeBookingConfirmationEmail(Booking booking, User user, string host, int port)
    {
        var gmailAddress = _options.GmailAddress;

        var htmlBody = $"""
                    <html>
                    <head>
                    </head>
                    <body>
                        <h2>Thank you for choosing us! Confirming Booking Reservation!</h2>
                        <p>We will be waiting for your arrival, Mr./Mrs. {user.FirstName} {user.LastName}, on the {booking.StartDate} at {booking.HotelName}.</p>
                        <p>The room number is {booking.RoomName}</p>
                        <p>Have a great day!<p>
                    </body>
                    </html>
                    """;

        var emailMessage = new Email();

        emailMessage.From = new Email.MailboxAddress
        {
            Name = "Hotel Management",
            Address = gmailAddress,
        };
        emailMessage.To = new Email.MailboxAddress
        {
            Name = $"{user.LastName} {user.FirstName}",
            Address = user.Email,
        };
        emailMessage.Subject = "[HMS] Booking Confirmation";
        emailMessage.Body = htmlBody;

        await Send(emailMessage);
    }

    public async Task ComposeConfirmationEmail(User user, string host, int port)
    {
        var activationToken = Guid.NewGuid().ToString();
        var activationLink = $"https://{host}:{port}/Authentication/Activate?token={activationToken}&email={user.Email}";

        user.ActivationToken = activationToken;
        user.TokenGenerationTime = DateTime.UtcNow;

        var gmailAddress = _options.GmailAddress;

        var htmlBody = $"""
                    <html>
                    <head>
                    </head>
                    <body>
                        <h2>Thank you for choosing us! </h2>
                        <p>Your email address was used to create an account on our application</p>
                        <p>Your data:</p>
                        <p>Username {user.Username}</p>
                        <p>Last name {user.LastName}</p>
                        <p>First name {user.FirstName}</p>
                        <p>Birth date {user.BirthDate.Date}</p>
                        <p>Gender {user.Gender}</p>
                        <a href='{activationLink}'> Activate your account here.</p>
                    </body>
                    </html>
                    """;

        var emailMessage = new Email();

        emailMessage.From = new Email.MailboxAddress
        {
            Name = "Hotel Management",
            Address = gmailAddress,
        };
        emailMessage.To = new Email.MailboxAddress
        {
            Name = $"{user.LastName} {user.FirstName}",
            Address = user.Email,
        };
        emailMessage.Subject = "Activate Your Account";
        emailMessage.Body = htmlBody;

        await Send(emailMessage);
    }

    public async Task Send(Email emailMessage)
    {
        var gmailAddress = _options.GmailAddress;
        var password = _options.Password;
        var host = _options.Host;
        int port = int.Parse(_options.Port);

        //using (var client = new SmtpClient())
        //{
        //    try
        //    {
        //        await client.ConnectAsync(host, port, false);
        //        await client.AuthenticateAsync(gmailAddress, password);
        //        await client.SendAsync(EmailToMimeMessage.ConvertedMail(emailMessage));
        //        await client.DisconnectAsync(true);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }
        //}
    }
}
