using HotelManagement.Models.Constants;
using HotelManagement.Models.DataModels;

namespace HotelManagement.DataAccess.IRepository;

public interface IBookingRepository : IAbstractRepository<Booking>
{
    Task<(List<Booking>, int)> GetBookingsByUser(User user, BookingSortType sortOn, bool isAscending, int pageSize, int pageIndex);
    Task<(List<Booking>, int)> GetAll(ReservationListingSortType sortAttribute, bool isAscending, int pageSize, int pageIndex, List<Hotel> hotels);
}
