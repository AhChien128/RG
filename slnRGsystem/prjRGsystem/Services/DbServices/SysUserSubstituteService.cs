using prjRGsystem.Managers.DbManagers;
using prjRGsystem.Context;

namespace prjRGsystem.Services.DbServices
{
    public class SysUserSubstituteService : SysUserSubstituteManager
    {
        public SysUserSubstituteService(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }
    }
}
