using HotelManagement.BusinessLogic.ILogic;
using HotelManagement.DataAccess.IRepository;

namespace HotelManagement.BusinessLogic.Logic;

public class RoleLogic : IRoleLogic
{
    private readonly IRoleRepository _roleRepository;

    public RoleLogic(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Models.DataModels.DBRole> GetRoleByName(Models.Constants.Role name)
    {
        return await _roleRepository.GetRoleByName(name);
    }

    public IEnumerable<Models.DataModels.DBRole> GetAssignableRoles(bool isOwner)
    {
        var roles = isOwner ? new List<Models.Constants.Role> { Models.Constants.Role.Employee, Models.Constants.Role.Manager } 
        : new List<Models.Constants.Role> { Models.Constants.Role.Employee };

        return _roleRepository.GetAssignableRoles(roles);
    }
}
