using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using HotelManagement.BusinessLogic.ILogic;
using HotelManagement.Models.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using static Google.Apis.Drive.v3.DriveService;

namespace HotelManagement.BusinessLogic.Logic;

public class GoogleDriveImageUploaderService : IFileStorageService
{

    private readonly GoogleDriveOptions _googleDriveOptions;
    private readonly DriveService _driveService;

    public GoogleDriveImageUploaderService(IOptions<GoogleDriveOptions> options, DriveService driveService)
    {
        _googleDriveOptions = options.Value;
        _driveService = driveService;
    }

    public async Task<ImageUrl> UploadImage(Stream fileContent)
    {
        var folderId = _googleDriveOptions.FolderId;

        var fileMetaData = new Google.Apis.Drive.v3.Data.File()
        {
            Parents = new List<string> { folderId },
        };

        FilesResource.CreateMediaUpload request;

        request = _driveService.Files.Create(fileMetaData, fileContent, "");
        request.Fields = "id";
        try
        {
            request.Upload();
        }
        catch (Exception ex)
        {
            throw new Exception("Upload failed!");
        }

        var uploadedFile = request.ResponseBody;

        return GoogleDriveAPIHelper.GetImageUrl(_googleDriveOptions.DrivePrefix, uploadedFile.Id);
    }
}

public class GoogleDriveAPIHelper
{
    public static ImageUrl GetImageUrl(string prefix, string id)
    {
        return new ImageUrl { Url = prefix + id };
    }
}
