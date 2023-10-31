using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models.DataModels;

public class Room
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    public int Name { get; set; }

    [Required]
    public int NumberOfPeople { get; set; }

    [Required]
    public int Price { get; set; }

    [Required]
    public bool AllowsSmoking { get; set; }

    [Required]
    public bool AllowsDogs { get; set; }

    [Required]
    public bool IsReserved { get; set; }

    public bool IsDeleted { get; set; }

    public Guid HotelId { get; set; }

    public Hotel Hotel { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();

}
