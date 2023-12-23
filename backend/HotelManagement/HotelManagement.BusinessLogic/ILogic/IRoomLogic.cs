using HotelManagement.Models.Constants;
using HotelManagement.Models.DataModels;
using HotelManagement.Models.ViewModels;

namespace HotelManagement.BusinessLogic.ILogic;

public interface IRoomLogic
{
    void AddRoom(Room room);
    Task<List<Room>> GetAvailableRooms(Guid id, DateOnly startDate, DateOnly endDate);
    //Task<PaginatedList<RoomDetailsViewModel>> GetByHotelId(Guid hotelId, RoomListingSortType sortAttribute, bool isAscending, int pageSize, int pageIndex, bool? allowsSmoking, bool? allowsDogs);
    Task<Room> GetById(Guid roomId);
}
