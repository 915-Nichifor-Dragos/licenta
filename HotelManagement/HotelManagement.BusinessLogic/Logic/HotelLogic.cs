using HotelManagement.BusinessLogic.Converters;
using HotelManagement.BusinessLogic.ILogic;
using HotelManagement.DataAccess.IRepository;
using HotelManagement.Models.Constants;
using HotelManagement.Models.DataModels;
using HotelManagement.Models.Options;
using HotelManagement.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace HotelManagement.BusinessLogic.Logic;

public class HotelLogic : IHotelLogic
{
    private readonly IHotelRepository _repository;
    private readonly IFileStorageService _fileStorageService;
    private readonly GoogleDriveOptions _driveOptions;
    private readonly ProfilePictureOptions _profilePictureOptions;

    public HotelLogic(IHotelRepository repository, IFileStorageService fileStorageService, IOptions<GoogleDriveOptions> configuration, IOptions<ProfilePictureOptions> options)
    {
        _repository = repository;
        _fileStorageService = fileStorageService;
        _driveOptions = configuration.Value;
        _profilePictureOptions = options.Value;
    }

    public async Task<List<Hotel>> GetAll(int count, User user)
    {
        return await _repository.GetHotels(count, user);
    }

    public async Task<IEnumerable<Hotel>> GetHotelsBySubstringAndCount(int takeCount, User authenticatedUser, string? name)
    {
        return await _repository.GetHotelsBySubstringAndCount(takeCount, authenticatedUser, name);
    }

    public async Task<(List<HotelManagementHotelView>, int count)> GetHotelsByOwner(
        User user,
        int pageIndex, 
        int pageSize, 
        HotelSortType sortAttribute, 
        bool isAscending)
    {

        var tupleItemsHotelsAndCount = await _repository.GetHotelsByOwner(user, pageIndex, pageSize, sortAttribute, isAscending);

        var hotelsToSendInView = tupleItemsHotelsAndCount.Item1
            .Select(HotelConverter.FromHotelToHotelManagementHotelView)
            .ToList();
        var count = tupleItemsHotelsAndCount.Item2;

        return (hotelsToSendInView, count);
    }

    public async Task<Hotel> GetById(Guid hotelId)
    {
        return await _repository.GetById(hotelId);
    }

    public async Task AddUserToHotel(Hotel hotel, User user)
    {
        await _repository.AddUserToHotel(hotel, user);
    }

    public async Task AddHotel(Hotel hotel, IFormFile profilePicture)
    {
        _repository.Add(hotel);
        var imageUrl = await UploadProfilePicture(profilePicture);
        hotel.ImageUrl = imageUrl;

        await _repository.SaveChanges();
    }

    public async Task UpdateHotel(Hotel hotel)
    {
        await _repository.Update(hotel);
    }

    public async Task<int> GetNextHotelRoomNumber(Guid hotelId)
    {
        return await _repository.GetNextHotelRoomNumber(hotelId);
    }

    public async Task<List<Hotel>> GetAvailableHotels(DateOnly startDate, DateOnly endDate)
    {
        return await _repository.GetAvailableHotels(startDate.ToDateTime(TimeOnly.MinValue), endDate.ToDateTime(TimeOnly.MinValue));
    }

    public async Task<List<Hotel>> GetByListId(List<Guid> hotelIds)
    {
        return await _repository.GetByListId(hotelIds);
    }

    public async Task<string> UploadProfilePicture(IFormFile profilePicture)
    {
        if (profilePicture != null)
        {
            try
            {
                var imageUrl = await _fileStorageService.UploadImage(profilePicture.OpenReadStream());
                
                if (imageUrl == null)
                {
                    throw new Exception("Could not upload picture");
                }

                return imageUrl.Url;
            }
            catch (Exception ex)
            {
                return _driveOptions.DefaultProfilePicture;
            }
        }

        return _driveOptions.DefaultProfilePicture;
    }

    public async Task DeletePicture(Guid id)
    {
        var hotel = await _repository.GetById(id);

        if (hotel == null)
        {
            throw new Exception("The user doesn't exit!");
        }

        hotel.ImageUrl = _driveOptions.DefaultHotelPicture;

        await _repository.Update(hotel);
    }
}
