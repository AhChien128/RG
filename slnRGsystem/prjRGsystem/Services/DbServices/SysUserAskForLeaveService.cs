using prjRGsystem.Managers.DbManagers;
using prjRGsystem.Context;

namespace prjRGsystem.Services.DbServices
{
    public class SysUserAskForLeaveService : SysUserAskForLeaveManager
    {
        public SysUserAskForLeaveService(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }
    }
}
