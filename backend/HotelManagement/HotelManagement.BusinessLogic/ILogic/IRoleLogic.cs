namespace HotelManagement.BusinessLogic.ILogic;

public interface IRoleLogic
{
    public Task<Models.DataModels.DBRole> GetRoleByName(Models.Constants.Role name);
    public IEnumerable<Models.DataModels.DBRole> GetAssignableRoles(bool isOwner);
}
