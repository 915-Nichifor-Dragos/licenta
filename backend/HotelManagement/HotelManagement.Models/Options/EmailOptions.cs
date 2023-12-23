namespace HotelManagement.Models.Options;

public sealed class EmailOptions
{
    public string GmailAddress { get; set; }

    public string Password { get; set; }

    public string Host { get; set; }

    public string Port { get; set; }
}
