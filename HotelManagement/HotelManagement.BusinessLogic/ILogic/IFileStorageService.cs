using HotelManagement.Models.Options;

namespace HotelManagement.BusinessLogic.ILogic;

public interface IFileStorageService
{
    public Task<ImageUrl> UploadImage(Stream fileContent);
}
