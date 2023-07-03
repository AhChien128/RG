using prjRGsystem.Context;
using prjRGsystem.Managers.DbManagers;

namespace prjRGsystem.Services.DbServices
{
    public class SysDepartmentService : SysDepartmentManager
    {
        //private readonly NoticeService noticeService;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly SysUserManager sysUserManager;
        public SysDepartmentService(RGPropertyContext _db,
            IHttpContextAccessor _httpContextAccessor,
            //NoticeService _noticeService,
            IServiceScopeFactory _serviceScopeFactory,
        SysUserManager _sysUserManager) : base(_db, _httpContextAccessor)
        {
            //noticeService = _noticeService;
            serviceScopeFactory = _serviceScopeFactory;
            sysUserManager = _sysUserManager;
        }
 
    }
}
