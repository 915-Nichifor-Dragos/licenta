namespace HotelManagement.Models.DataModels;

public class Hotel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Location { get; set; }

    public bool IsAvailable { get; set; }

    public int NumberOfEmployees { get; set; }

    public string Description { get; set; }

    public Guid OwnerId { get; set; }

    public User Owner { get; set; }

    public double Earnings { get; set; }

    public bool HasFreeWiFi { get; set; }

    public bool HasParking { get; set; }

    public bool HasPool { get; set; }

    public bool HasSauna { get; set; }

    public bool HasRestaurant { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<UserHotels> UserHotels { get; set; } = new HashSet<UserHotels>();

    public virtual ICollection<Room> Rooms { get; set; } = new HashSet<Room>();

    public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
}
