using HotelManagement.Models.DataModels;

namespace HotelManagement.BusinessLogic.ILogic;

public interface IEmailService
{
    Task ComposeConfirmationEmail(User user, string host, int port);
    Task Send(Email emailMessage);
    Task ComposeBookingConfirmationEmail(Booking booking, User user, string host, int port);
}
