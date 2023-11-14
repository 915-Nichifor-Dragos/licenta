using HotelManagement.DataAccess.Contexts;
using HotelManagement.DataAccess.IRepositories;
using HotelManagement.DataAccess.IRepository;
using HotelManagement.Models.Constants;
using HotelManagement.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace HotelManagement.DataAccess.Repository;

public class HotelRepository : AbstractRepository<Hotel>, IHotelRepository
{
    private readonly HotelManagementContext _context;

    private readonly DbSet<Hotel> _hotels;

    public HotelRepository(HotelManagementContext dbContext) : base(dbContext)
    {
        _context = dbContext;
        _hotels = dbContext.Set<Hotel>();
    }

    public async Task<Hotel?> GetById(Guid id)
    {
        return await _context.Hotels
            .Include(u => u.UserHotels)
            .Include(u => u.Bookings)
                .ThenInclude(b => b.Room)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public void Add(Hotel hotel)
    {
        _context.Hotels.Add(hotel);
        _context.SaveChanges();
    }

    public async Task Update(Hotel hotel)
    {
        _context.Hotels.Update(hotel);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var hotel = _context.Hotels.Find(id);

        if (hotel != null)
        {
            hotel.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> SaveChanges()
    {
        return await _context
            .SaveChangesAsync();
    }

    public async Task<List<Hotel>> GetHotels(int count, User user)
    {
        if (user.Role.Name == Models.Constants.Role.Owner)
        {
            return await _context.Hotels
                .Take(count)
                .ToListAsync();
        }

        return user.UserHotels
            .Select(u => u.Hotel)
            .Take(count)
            .ToList();
    }

    public async Task AddUserToHotel(Hotel hotel, User user)
    {
        if (hotel != null && user != null)
        {
            var existingUserHotel = await _context.UserHotels
                .FirstOrDefaultAsync(uh => uh.HotelsId == hotel.Id && uh.UsersId == user.Id);

            if (existingUserHotel != null)
            {
                existingUserHotel.RegistrationDate = DateTime.Now;
                existingUserHotel.IsDeleted = false;
            }

            var userHotel = new UserHotels
            {
                UsersId = user.Id,
                HotelsId = hotel.Id,
                RegistrationDate = DateTime.UtcNow,
                IsDeleted = false
            };

            hotel.UserHotels.Add(userHotel);

            await _context.SaveChangesAsync();
        }
        else
        {
            throw new Exception("Hotel not found.");
        }
    }

    public async Task<(List<Hotel>, int)>? GetHotelsByOwner(
        User user, 
        int pageIndex, 
        int pageSize, 
        HotelSortType sortAttribute, 
        bool isAscending)
    {

        var query = _context.Hotels
            .Where(u => u.OwnerId == user.Id)
            .Include(u => u.UserHotels)
            .Select(h => new Hotel
            {
                Id = h.Id,
                Name = h.Name,
                IsAvailable = h.IsAvailable,
                NumberOfEmployees = h.UserHotels.Count,
                Location = h.Location,
            })
            .AsQueryable();

        query = sortAttribute switch
        {
            HotelSortType.Name => isAscending ? query.OrderBy(u => u.Name)
                                : query.OrderByDescending(u => u.Name),
            HotelSortType.Location => isAscending ? query.OrderBy(u => u.Location)
                                : query.OrderByDescending(u => u.Location),
            HotelSortType.NumberOfEmployees => isAscending ? query.OrderBy(u => u.NumberOfEmployees)
                                : query.OrderByDescending(u => u.NumberOfEmployees),
            _ => isAscending ? query.OrderBy(u => u.Name)
                                 : query.OrderByDescending(u => u.Name),
        };

        var totalHotelCount = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        return (items, totalHotelCount);
    }

    public async Task<int> GetNextHotelRoomNumber(Guid hotelId)
    {
        var hotel = await _context.Hotels
             .Include(h => h.Rooms)
             .FirstOrDefaultAsync(h => h.Id == hotelId);

        if (hotel == null)
        {
            throw new Exception("Hotel does not exist");
        }

        int nextRoomNumber = hotel.Rooms
            .OrderByDescending(r => r.Name)
            .Select(r => r.Name)
            .FirstOrDefault();

        return nextRoomNumber + 1;
    }

    public async Task<IEnumerable<Hotel>> GetHotelsBySubstringAndCount(int takeCount, User authenticatedUser, string? name)
    {
        if (authenticatedUser.Role.Name.Equals(Role.Owner))
        {
            if (string.IsNullOrEmpty(name))
            {
                return await _context.Hotels
                    .Take(takeCount)
                    .ToListAsync();
            }

            return await _context.Hotels
                .Where(hotel => hotel.Name.ToLower().Contains(name))
                .Take(takeCount)
                .ToListAsync();
        }

        List<Hotel> hotels = authenticatedUser.UserHotels
           .Select(uh => uh.Hotel)
           .ToList();

        if (string.IsNullOrEmpty(name))
        {
            return hotels.Take(takeCount);
        }

        return hotels
            .Where(hotel => hotel.Name.ToLower().Contains(name))
            .Take(takeCount);
    }

    public async Task<List<Hotel>> GetAvailableHotels(DateTime startDate, DateTime endDate)
    {
        return await _context.Hotels
            .Where(h => h.IsAvailable && h.Rooms
                .Any(r => r.Bookings
                    .Any(b => (endDate <= b.StartDate) || (startDate >= b.EndDate)) || !r.Bookings.Any()
                )
            )
          .ToListAsync();
    }

    public async Task<List<Hotel>> GetByListId(List<Guid> hotelIds)
    {
        return await _context.Hotels
            .Where(h => hotelIds.Contains(h.Id))
            .Include(h => h.UserHotels)
            .ToListAsync();
    }
}
