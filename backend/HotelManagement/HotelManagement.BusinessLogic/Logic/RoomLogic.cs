using HotelManagement.BusinessLogic.Converters;
using HotelManagement.BusinessLogic.ILogic;
using HotelManagement.DataAccess.IRepository;
using HotelManagement.DataAccess.Repository;
using HotelManagement.Models.Constants;
using HotelManagement.Models.DataModels;
using HotelManagement.Models.ViewModels;

namespace HotelManagement.BusinessLogic.Logic;

public class RoomLogic : IRoomLogic
{
    private readonly IRoomRepository _repository;

    public RoomLogic(IRoomRepository repository)
    {
        _repository = repository;
    }

    public void AddRoom(Room room)
    {
        _repository.Add(room);
    }

    public async Task<List<Room>> GetAvailableRooms(Guid roomId, DateOnly startDate, DateOnly endDate)
    {
        return await _repository.GetAvailableRooms(roomId, startDate.ToDateTime(TimeOnly.MinValue), endDate.ToDateTime(TimeOnly.MinValue));
    }

    //public async Task<PaginatedList<RoomDetailsViewModel>> GetByHotelId(Guid hotelId, RoomListingSortType sortAttribute, bool isAscending, int pageSize, int pageIndex, bool? allowsSmoking, bool? allowsDogs)
    //{
    //    var tupleItemsRoomsAndCount = await _repository.GetByHotelId(hotelId, sortAttribute, isAscending, pageSize, pageIndex, allowsSmoking, allowsDogs);

    //    var rooms = tupleItemsRoomsAndCount.Item1.Select(u => u.ConvertRoom()).ToList();
    //    var count = tupleItemsRoomsAndCount.Item2;

    //    return new PaginatedList<RoomDetailsViewModel>(rooms, count, pageIndex, pageSize, sortAttribute.ToString(), isAscending);
    //}

    public async Task<Room> GetById(Guid roomId)
    {
        return await _repository.GetById(roomId);
    }
}
