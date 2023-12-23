using HotelManagement.DataAccess.Contexts;
using HotelManagement.DataAccess.IRepositories;
using HotelManagement.DataAccess.IRepository;
using HotelManagement.Models.Constants;
using HotelManagement.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HotelManagement.DataAccess.Repository;

public class RoomRepository : AbstractRepository<Room>, IRoomRepository
{
    private readonly HotelManagementContext _context;

    public RoomRepository(HotelManagementContext dbContext) : base(dbContext)
    {
        _context = dbContext;
    }
    public void Add(Room room)
    {
        _context.Rooms.Add(room);
        _context.SaveChanges();
    }

    public async Task Update(Room room)
    {
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var room = _context.Rooms.Find(id);

        if (room != null)
        {
            room.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> SaveChanges()
    {
        return await _context
            .SaveChangesAsync();
    }

    public async Task<(List<Room>, int)> GetByHotelId(Guid hotelId, RoomListingSortType sortAttribute, bool isAscending, int pageSize, int pageIndex, bool? allowsSmoking, bool? allowsDogs)
    {
        var query = _context.Rooms
            .Where(u => u.HotelId == hotelId && u.AllowsSmoking == allowsSmoking && u.AllowsDogs == allowsDogs)
            .Include(u => u.Bookings)
            .AsQueryable();

        switch (sortAttribute)
        {
            case RoomListingSortType.Number:
                query = isAscending ? query.OrderBy(u => u.Name)
                    : query.OrderByDescending(u => u.Name);
                break;

            case RoomListingSortType.People:
                query = isAscending ? query.OrderBy(u => u.NumberOfPeople)
                    : query.OrderByDescending(u => u.NumberOfPeople);
                break;

            case RoomListingSortType.Price:
                query = isAscending ? query.OrderBy(u => u.Price)
                    : query.OrderByDescending(u => u.Price);
                break;

            case RoomListingSortType.AllowsSmoking:
                query = isAscending ? query.OrderBy(u => u.AllowsSmoking)
                    : query.OrderByDescending(u => u.AllowsSmoking);
                break;

            case RoomListingSortType.AllowsDogs:
                query = isAscending ? query.OrderBy(u => u.AllowsDogs)
                    : query.OrderByDescending(u => u.AllowsDogs);
                break;

            default:
                query.OrderBy(u => u.Name);
                break;
        }

        var totalUserCount = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        return (items, totalUserCount);
    }

    public async Task<List<Room>> GetAvailableRooms(Guid hotelId, DateTime startDate, DateTime endDate)
    {
        return await _context.Rooms      
            .Where(h => h.HotelId.Equals(hotelId) && (h.Bookings                    
                    .Any(b => (endDate <= b.StartDate) || (startDate >= b.EndDate)) || !h.Bookings.Any()))
            .ToListAsync();
    }

    public async Task<bool> IsRoomAvailableInInterval(Booking booking)
    {
        return !((await GetAvailableRooms(booking.HotelId, booking.StartDate, booking.EndDate)).Where(r => r.Id.Equals(booking.RoomId)).IsNullOrEmpty());
    }
}
