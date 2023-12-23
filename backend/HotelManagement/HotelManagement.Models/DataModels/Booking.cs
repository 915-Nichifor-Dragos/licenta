using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models.DataModels;

public class Booking
{
    public Guid Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public Guid HotelId { get; set; }

    public Hotel Hotel { get; set; }

    public Guid RoomId { get; set; }

    public Room Room { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    public bool IsDeleted { get; set; }

    public string HotelName { get; set; }

    public int RoomName { get; set; }
}
