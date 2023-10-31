namespace HotelManagement.DataAccess.IRepository;

public interface IRoleRepository
{
    public Task<Models.DataModels.DBRole> GetRoleByName(Models.Constants.Role name);
    IEnumerable<Models.DataModels.DBRole> GetAssignableRoles(List<Models.Constants.Role> roles);
}
