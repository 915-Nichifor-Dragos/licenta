using HotelManagement.Models.Constants;
using HotelManagement.Models.DataModels;

namespace HotelManagement.DataAccess.IRepository;

public interface IRoomRepository
{
    void Add(Room entity);
    Task Update(Room room);
    Task Delete(Guid id);
    Task<int> SaveChanges();
    Task<(List<Room>, int)> GetByHotelId(Guid hotelId, RoomListingSortType sortAttribute, bool isAscending, int pageSize, int pageIndex, bool? allowsSmoking, bool? allowsDogs);
    Task<Room> GetById(Guid id);
    Task<List<Room>> GetAvailableRooms(Guid hotelId, DateTime startDate, DateTime endDate);
    Task<bool> IsRoomAvailableInInterval(Booking booking);
}
