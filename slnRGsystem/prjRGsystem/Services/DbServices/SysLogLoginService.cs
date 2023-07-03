using prjRGsystem.Context;
using prjRGsystem.Managers.DbManagers;

namespace prjRGsystem.Services.DbServices
{
    public class SysLogLoginService : SysLogLoginManager
    {
        public SysLogLoginService(RGPropertyContext _db,
            IHttpContextAccessor _httpContextAccessor
            ) : base(_db, _httpContextAccessor)
        {

        }

    }
}
