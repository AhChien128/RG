using prjRGsystem.Context;
using prjRGsystem.Managers.DbManagers;

namespace prjRGsystem.Services.DbServices
{
    public class SysUsersRolesService : SysUsersRolesManager
    {
        public SysUsersRolesService(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }
    }
}
