using HotelManagement.Models.DataModels;
using HotelManagement.Models.ViewModels;

namespace HotelManagement.BusinessLogic.Converters;

public static class HotelConverter
{
    public static HotelManagementHotelView FromHotelToHotelManagementHotelView(Hotel hotel)
    {
        return new HotelManagementHotelView
        {
            Id = hotel.Id,
            Location = hotel.Location,
            IsAvailable = hotel.IsAvailable,
            Name = hotel.Name,
            NumberOfEmployees = hotel.NumberOfEmployees,
        };
    }
}
