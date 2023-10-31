using HotelManagement.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models.DataModels;

public class User
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(30)]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string LastName { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [MaxLength(100)]
    public string? Address { get; set; }

    [MaxLength(1000)]
    public string? Bio { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; }

    public string? ActivationToken { get; set; }

    public DateTime? TokenGenerationTime { get; set; }

    public bool IsDeleted { get; set; }

    public int RoleId { get; set; }
    public DBRole Role { get; set; }

    public virtual ICollection<UserHotels> UserHotels { get; set; } = new HashSet<UserHotels>();

    public virtual ICollection<Hotel> OwnedHotels { get; set; } = new HashSet<Hotel>();

    public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
}
