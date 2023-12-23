using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HotelManagement.Models.Constants;

public static class EnumHelper
{
    public static string GetDisplayName(Enum value)
    {
        var enumType = value.GetType();
        var enumValueName = Enum.GetName(enumType, value);
        var memberInfo = enumType.GetMember(enumValueName)[0];
        var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();

        return displayAttribute?.Name ?? enumValueName;
    }
}
