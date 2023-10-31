using HotelManagement.DataAccess.Contexts;
using HotelManagement.DataAccess.IRepositories;
using HotelManagement.DataAccess.IRepository;
using HotelManagement.Models.Constants;
using HotelManagement.Models.DataModels;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.DataAccess.Repository;

public class BookingRepository : AbstractRepository<Booking>, IBookingRepository
{
    private readonly HotelManagementContext _context;

    public BookingRepository(HotelManagementContext dbContext) : base(dbContext)
    {
        _context = dbContext;
    }

    public async Task<(List<Booking>, int)> GetAll(ReservationListingSortType sortAttribute, bool isAscending, int pageSize, int pageIndex, List<Hotel> hotels)
    {
        var hotelIds = hotels.Select(h => h.Id).ToList();

        var query = _context.Bookings
                         .Where(b => hotelIds.Contains(b.HotelId))
                         .Include(b => b.Hotel)
                         .Include(b => b.Room)
                         .AsQueryable();

        switch (sortAttribute)
        {
            case ReservationListingSortType.Hotel:
                query = isAscending ? query.OrderBy(b => b.Hotel.Name)
                    : query.OrderByDescending(b => b.Hotel.Name);
                break;

            case ReservationListingSortType.Room:
                query = isAscending ? query.OrderBy(b => b.Room.Name)
                    : query.OrderByDescending(b => b.Room.Name);
                break;

            case ReservationListingSortType.StartDate:
                query = isAscending ? query.OrderBy(b => b.StartDate)
                    : query.OrderByDescending(b => b.StartDate);
                break;

            case ReservationListingSortType.EndDate:
                query = isAscending ? query.OrderBy(b => b.EndDate)
                    : query.OrderByDescending(b => b.EndDate);
                break;

            default:
                query = isAscending ? query.OrderBy(b => b.Id)
                    : query.OrderByDescending(b => b.Id);
                break;
        }

        var totalUserCount = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        return (items, totalUserCount);
    }
    public override async Task Delete(Booking booking)
    {
        booking = await _context.Bookings.FindAsync(booking.Id);

        if (booking != null)
        {
            booking.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<(List<Booking>, int)> GetBookingsByUser(User user, BookingSortType sortOn, bool isAscending, int pageSize, int pageIndex)
    {
        var query = _context.Bookings
            .Include(u => u.Hotel)
            .Include(u => u.Room)
            .Where(u => u.UserId == user.Id)
            .AsQueryable();

        switch (sortOn)
        {
            case BookingSortType.Hotel:
                query = isAscending ? query.OrderBy(u => u.Hotel.Name)
                    : query.OrderByDescending(u => u.Hotel.Name);
                break;

            case BookingSortType.Room:
                query = isAscending ? query.OrderBy(u => u.Room.Name)
                    : query.OrderByDescending(u => u.Room.Name);
                break;

            case BookingSortType.Days:
                query = isAscending ? query.OrderBy(u => EF.Functions.DateDiffDay(u.StartDate, u.EndDate))
                    : query.OrderByDescending(u => EF.Functions.DateDiffDay(u.StartDate, u.EndDate));
                break;

            case BookingSortType.EndDate:
                query = isAscending ? query.OrderBy(u => u.EndDate)
        : query.OrderByDescending(u => u.EndDate);
                break;

            default:
                break;
        }

        var totalUserCount = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        return (items, totalUserCount);
    }

    public override async Task<Booking> GetById(Guid id)
    {
        return await _dbSet
            .Include(b => b.Hotel)
            .Include(b => b.Room)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id.Equals(id));
    }
}