using prjRGsystem.Context;
using prjRGsystem.Managers.DbManagers;

namespace prjRGsystem.Services.DbServices
{
    public class SysLogDataChangeService : SysLogDataChangeManager
    {
        public SysLogDataChangeService(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }
    }
}
