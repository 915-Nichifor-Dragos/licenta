using HotelManagement.Models.DataModels;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.DataAccess.Contexts;

public class HotelManagementContext : DbContext
{
    public HotelManagementContext(DbContextOptions<HotelManagementContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<DBRole> Roles { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<UserHotels> UserHotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DBRole>().HasKey(r => r.Id);

        modelBuilder.Entity<UserHotels>()
               .HasKey(uh => new { uh.UsersId, uh.HotelsId });

        modelBuilder.Entity<UserHotels>()
            .HasOne(uh => uh.User)
            .WithMany(u => u.UserHotels)
            .HasForeignKey(uh => uh.UsersId);

        modelBuilder.Entity<UserHotels>()
            .HasOne(uh => uh.Hotel)
            .WithMany(h => h.UserHotels)
            .HasForeignKey(uh => uh.HotelsId);
        modelBuilder.Entity<UserHotels>().HasQueryFilter(u => !u.IsDeleted);

        modelBuilder.Entity<Hotel>().HasKey(h => h.Id);
        modelBuilder.Entity<Hotel>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<Hotel>().Property(x => x.Name);
        modelBuilder.Entity<Hotel>().Property(x => x.Location);
        modelBuilder.Entity<Hotel>().Property(x => x.IsAvailable);
        modelBuilder.Entity<Hotel>().Property(x => x.Description);
        modelBuilder.Entity<Hotel>()
                .HasOne(h => h.Owner)
                .WithMany(u => u.OwnedHotels)
                .HasForeignKey(h => h.OwnerId)
                .IsRequired(false);

        modelBuilder.Entity<Hotel>().Ignore(h => h.NumberOfEmployees);
        modelBuilder.Entity<Hotel>().Ignore(h => h.Earnings);

        modelBuilder.Entity<Hotel>().Property(h => h.HasFreeWiFi);
        modelBuilder.Entity<Hotel>().Property(h => h.HasParking);
        modelBuilder.Entity<Hotel>().Property(h => h.HasPool);
        modelBuilder.Entity<Hotel>().Property(h => h.HasSauna);
        modelBuilder.Entity<Hotel>().Property(h => h.HasRestaurant);
        modelBuilder.Entity<Hotel>().Property(h => h.ImageUrl);
        modelBuilder.Entity<Hotel>().HasQueryFilter(u => !u.IsDeleted);

        modelBuilder.Entity<User>().HasKey(x => x.Id);
        modelBuilder.Entity<User>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<User>().Property(x => x.Username);
        modelBuilder.Entity<User>().Property(x => x.Password);
        modelBuilder.Entity<User>().Property(x => x.FirstName);
        modelBuilder.Entity<User>().Property(x => x.LastName);
        modelBuilder.Entity<User>().Property(x => x.Email);
        modelBuilder.Entity<User>().Property(x => x.BirthDate);
        modelBuilder.Entity<User>().Property(x => x.Gender);
        modelBuilder.Entity<User>().Property(x => x.Address);
        modelBuilder.Entity<User>().Property(x => x.Bio);
        modelBuilder.Entity<User>().Property(x => x.ImageUrl);
        modelBuilder.Entity<User>().HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);

        modelBuilder.Entity<Room>().HasKey(x => x.Id);
        modelBuilder.Entity<Room>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<Room>().Property(x => x.Name);
        modelBuilder.Entity<Room>().Property(x => x.Price);
        modelBuilder.Entity<Room>().Property(x => x.NumberOfPeople);
        modelBuilder.Entity<Room>().Property(x => x.AllowsSmoking);
        modelBuilder.Entity<Room>().Property(x => x.AllowsDogs);
        modelBuilder.Entity<Room>().Property(x => x.IsReserved);
        modelBuilder.Entity<Room>()
             .HasOne(r => r.Hotel)
             .WithMany(h => h.Rooms)
             .HasForeignKey(h => h.HotelId);
        modelBuilder.Entity<Room>().HasQueryFilter(u => !u.IsDeleted);

        modelBuilder.Entity<Booking>().HasKey(x => x.Id);
        modelBuilder.Entity<Booking>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<Booking>().Property(x => x.StartDate);
        modelBuilder.Entity<Booking>().Property(x => x.EndDate);
        modelBuilder.Entity<Booking>().Ignore(h => h.HotelName);
        modelBuilder.Entity<Booking>().Ignore(h => h.RoomName);

        modelBuilder.Entity<Booking>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Bookings)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
                .HasOne(r => r.User)
                .WithMany(h => h.Bookings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
                .HasOne(r => r.Room)
                .WithMany(h => h.Bookings)
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Booking>().HasQueryFilter(u => !u.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }
}
