namespace HotelManagement.Models.DataModels;

public class UserHotels
{
    public Guid UsersId { get; set; }
    public User User { get; set; }

    public Guid HotelsId { get; set; }
    public Hotel Hotel { get; set; }

    public DateTime RegistrationDate { get; set; }

    public bool IsDeleted { get; set; }
}