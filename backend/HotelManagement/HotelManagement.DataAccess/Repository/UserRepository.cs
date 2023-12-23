using HotelManagement.DataAccess.Contexts;
using HotelManagement.DataAccess.IRepositories;
using HotelManagement.DataAccess.IRepository;
using HotelManagement.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using HotelManagement.Models.Constants;

namespace HotelManagement.DataAccess.Repository;

public class UserRepository : AbstractRepository<User>, IUserRepository
{
    private readonly HotelManagementContext _context;

    public UserRepository(HotelManagementContext dbContext) : base(dbContext)
    {
        _context = dbContext;
    }

    public async Task<User?> GetById(Guid id)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public async Task<User?> GetByUsername(string username)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(user => user.Username.Equals(username));
    }

    public async Task<User?> GetByUsernameWithHotels(string username)
    {
        return await _context.Users
            .Include(u => u.Role)
            .Include(u => u.UserHotels)
            .ThenInclude(uh => uh.Hotel)
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Username.Equals(username));
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public async Task Delete(Guid id)
    {
        var user = _context.Users.Find(id);

        if (user != null)
        {
           user.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task Activate(Guid id)
    {
        var user = _context.Users.Find(id);
        if (user != null)
        {
            user.IsActive = true;
            user.ActivationToken = null;
            await _context.SaveChangesAsync();
        }
    }

    public bool Any(Func<User, bool> predicate)
    {
        return _context.Users.Any(predicate);
    }

    public async Task<bool> AnyAsync(Expression<Func<User, bool>> predicate)
    {
        return await _context.Users.AnyAsync(predicate);
    }

    public async Task<int> SaveChanges()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task AddHotelToUser(Hotel hotel, User user)
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

    public async Task<DateTime> GetRegistrationDate(Guid hotelId, Guid userId)
    {
        var registrationDate = await _context.UserHotels
            .Where(uh => uh.UsersId == userId && uh.HotelsId == hotelId)
            .Select(uh => uh.RegistrationDate)
            .OrderByDescending(uh => uh)
            .FirstOrDefaultAsync();


        return registrationDate;
    }

    public async Task<(List<User>, int)> GetByHotelId(Guid hotelId, UserListingSortType sortAttribute, bool isAscending, bool isOwner, int pageSize, int pageIndex)
    {
        var query = _context.Users
                        .Where(u => u.UserHotels.Any(h => h.HotelsId == hotelId) &&
                              ((isOwner && u.Role.Name != Models.Constants.Role.Owner) || (!isOwner && u.Role.Name == Models.Constants.Role.Employee)))
                        .Include(u => u.Role)
                        .Include(u => u.UserHotels)
                        .ThenInclude(uh => uh.Hotel)
                        .AsQueryable();

        query = sortAttribute switch
        {
            UserListingSortType.FirstName => isAscending ? query.OrderBy(u => u.FirstName)
                                : query.OrderByDescending(u => u.FirstName),
            UserListingSortType.LastName => isAscending ? query.OrderBy(u => u.LastName)
                                : query.OrderByDescending(u => u.LastName),
            UserListingSortType.Email => isAscending ? query.OrderBy(u => u.Email)
                                : query.OrderByDescending(u => u.Email),
            UserListingSortType.RegistrationDate => isAscending ? query.OrderBy(u => u.UserHotels.FirstOrDefault().RegistrationDate)
                    : query.OrderByDescending(u => u.UserHotels.FirstOrDefault().RegistrationDate),
            UserListingSortType.BirthDate => isAscending ? query.OrderBy(u => u.BirthDate)
                                : query.OrderByDescending(u => u.BirthDate),
            UserListingSortType.Role => isAscending ? query.OrderBy(u => u.Role)
                                : query.OrderByDescending(u => u.Role),
            _ => isAscending ? query.OrderBy(u => u.Username)
                                 : query.OrderByDescending(u => u.Username),
        };

        var totalUserCount = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        return (items, totalUserCount);
    }

    public async Task<(List<User>, int)> GetAll(UserListingSortType sortAttribute, bool isAscending, User user, int pageSize, int pageIndex)
    {
        var userHotelsIds = user.UserHotels.Select(uh => uh.HotelsId);

        var query = _context.Users
                        .Include(u => u.Role)
                        .Include(u => u.UserHotels)
                        .ThenInclude(uh => uh.Hotel)
                        .Where(u => (user.Role.Name.Equals(Role.Owner) && u.Role.Name != Role.Owner) ||
                                    (user.Role.Name.Equals(Role.Manager) && u.Role.Name == Role.Employee && u.UserHotels.Any(uh => userHotelsIds.Contains(uh.HotelsId))))
                        .Where(uh => uh.UserHotels.Count > 0)
                        .AsQueryable();

        query = sortAttribute switch
        {
            UserListingSortType.FirstName => isAscending ? query.OrderBy(u => u.FirstName)
                                : query.OrderByDescending(u => u.FirstName),
            UserListingSortType.LastName => isAscending ? query.OrderBy(u => u.LastName)
                                : query.OrderByDescending(u => u.LastName),
            UserListingSortType.Email => isAscending ? query.OrderBy(u => u.Email)
                                : query.OrderByDescending(u => u.Email),
            UserListingSortType.RegistrationDate => isAscending ? query.OrderBy(u => u.UserHotels.FirstOrDefault().RegistrationDate)
                    : query.OrderByDescending(u => u.UserHotels.FirstOrDefault().RegistrationDate),
            UserListingSortType.BirthDate => isAscending ? query.OrderBy(u => u.BirthDate)
                                : query.OrderByDescending(u => u.BirthDate),
            UserListingSortType.Role => isAscending ? query.OrderBy(u => u.Role)
                                : query.OrderByDescending(u => u.Role),
            _ => isAscending ? query.OrderBy(u => u.Username)
                                 : query.OrderByDescending(u => u.Username),
        };
        var totalUserCount = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        return (items, totalUserCount);
    }
}
