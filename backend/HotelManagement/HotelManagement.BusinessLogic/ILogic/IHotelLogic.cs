using HotelManagement.Models.Constants;
using HotelManagement.Models.DataModels;
using HotelManagement.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace HotelManagement.BusinessLogic.ILogic;

public interface IHotelLogic
{
    Task<List<Hotel>> GetAll(int count, User user);
    Task<IEnumerable<Hotel>> GetHotelsBySubstringAndCount(int takeCount, User authenticatedUser, string? name);
    Task<Hotel> GetById(Guid hotelId);
    Task AddUserToHotel(Hotel hotel, User user);
    Task<(List<HotelManagementHotelView>, int count)> GetHotelsByOwner(User user, int pageIndex, int pageSize, HotelSortType sortAttribute, bool isAscending);
    Task AddHotel(Hotel hotel, IFormFile profilePicture);
    Task UpdateHotel(Hotel hotel);
    Task<int> GetNextHotelRoomNumber(Guid hotelId);
    Task<List<Hotel>> GetAvailableHotels(DateOnly startDate, DateOnly endDate);
    Task<List<Hotel>> GetByListId(List<Guid> hotelIds);
    Task<string> UploadProfilePicture(IFormFile profilePicture);
    Task DeletePicture(Guid id);
    Task<bool> DeleteHotel(Guid id);
}
