using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models.Constants;

public enum Role
{
    [Display(Name = "Client")]
    Client,

    [Display(Name = "Employee")]
    Employee,

    [Display(Name = "Manager")]
    Manager,

    [Display(Name = "Owner")]
    Owner
}
