namespace HotelManagement.Models.DataModels;

public class DBRole
{
    public int Id { get; set; }

    public Constants.Role Name { get; set; }

    public HashSet<User> Users { get; set; } = new HashSet<User>();
}
