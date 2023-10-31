using HotelManagement.Models.Constants;
using HotelManagement.Models.DataModels;
using HotelManagement.Models.ViewModels;
using System.Linq.Expressions;

namespace HotelManagement.DataAccess.IRepository;

public interface IUserRepository
{
    Task<User?> GetById(Guid id);
    Task<User?> GetByEmail(string email);
    Task<User?> GetByUsername(string username);
    Task<User?> GetByUsernameWithHotels(string username);
    Task<(List<User>, int)> GetByHotelId(Guid hotelId, UserListingSortType sortAttribute, bool isAscending, bool isOwner, int pageSize, int pageIndex);
    Task<(List<User>, int)> GetAll(UserListingSortType sortAttribute, bool isAscending, User user, int pageSize, int pageIndex);
    void Add(User entity);
    void Update(User entity);
    Task Delete(Guid id);
    public Task Activate(Guid id);
    bool Any(Func<User, bool> predicate);
    Task<bool> AnyAsync(Expression<Func<User, bool>> predicate);
    Task<int> SaveChanges();
    Task AddHotelToUser(Hotel hotel, User user);
    Task<DateTime> GetRegistrationDate(Guid hotelId, Guid userId);
}
