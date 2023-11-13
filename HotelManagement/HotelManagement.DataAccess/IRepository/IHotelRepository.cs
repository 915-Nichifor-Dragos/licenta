using HotelManagement.Models.Constants;
using HotelManagement.Models.DataModels;

namespace HotelManagement.DataAccess.IRepository;

public interface IHotelRepository
{
    Task<Hotel> GetById(Guid id);
    void Add(Hotel entity);
    Task Update(Hotel entity);
    void Delete(Guid id);
    Task<int> SaveChanges();
    Task<List<Hotel>> GetHotels(int count, User user);
    Task<(List<Hotel>, int)>? GetHotelsByOwner(User user, int pageIndex, int pageSize, HotelSortType sortAttribute, bool isAscending);
    Task AddUserToHotel(Hotel hotel, User user);
    Task<int> GetNextHotelRoomNumber(Guid hotelId);
    Task<List<Hotel>> GetAvailableHotels(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Hotel>>GetHotelsBySubstringAndCount(int takeCount, User authenticatedUser, string? name);
    Task<List<Hotel>> GetByListId(List<Guid> hotelIds);
}
