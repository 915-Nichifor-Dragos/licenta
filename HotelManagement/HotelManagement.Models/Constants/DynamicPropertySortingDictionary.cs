using HotelManagement.Models.DataModels;
using System.Linq.Expressions;

namespace HotelManagement.Models.Constants
{
    public class DynamicPropertySortingDictionary
    {
        public static Dictionary<ValidOrderByParametersHotel, Expression<Func<Hotel, object>>> SortingDictionary = new Dictionary<ValidOrderByParametersHotel, Expression<Func<Hotel, object>>>()
        {
            { ValidOrderByParametersHotel.Name, h => h.Name },
            { ValidOrderByParametersHotel.Location, h=>h.Location },
            { ValidOrderByParametersHotel.NumberOfEmployees, h=>h.NumberOfEmployees },
        };
    }
}
