using prjRGsystem.Context;
using prjRGsystem.Extensions;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Services
{
    public class RolePrivilegeService
    {
        private readonly ISession? session;

        public RolePrivilegeService(
            IHttpContextAccessor _httpContextAccessor)
        {
            if (_httpContextAccessor.HttpContext is not null)
                session = _httpContextAccessor.HttpContext.Session;
        }

        public bool IsAdd()
        {
            return GetCurrentPageSysCodesTypes().isAdd ?? false;
        }

        public bool IsEdit()
        {
            return GetCurrentPageSysCodesTypes().isEdit ?? false;
        }

        public bool IsView()
        {
            return GetCurrentPageSysCodesTypes().isView ?? false;
        }

        public bool IsRemoved()
        {
            return GetCurrentPageSysCodesTypes().isRemoved ?? false;
        }

        public bool IsReport()
        {
            return GetCurrentPageSysCodesTypes().isReport ?? false;
        }

        public bool IsAudit()
        {
            return GetCurrentPageSysCodesTypes().isAudit ?? false;
        }
        public bool IsAdmin()
        {
            return GetCurrentPageSysCodesTypes().isAdmin ?? false;
        }

        public bool IsAllowDisplayForActionName(string appActionName)
        {
            return !GetCurrentPageTaskBlackNames().Contains(appActionName);
        }

        private SysRolesTasks GetCurrentPageSysCodesTypes()
        {
            return GetUserContext().currentPageSysCodesTypes ?? new();
        }

        private List<string> GetCurrentPageTaskBlackNames()
        {
            return GetUserContext().currentPageTaskBlackNames ?? new();
        }

        private UserContext GetUserContext()
        {
            if (session is not null)
            {
                UserContext? userContext = session.GetObjectFromJson<UserContext>(UserContext.SESSION_NAME.ToString());
                return userContext ??= new UserContext();
            }
            else
            {
                return new UserContext();
            }
        }

    }
}
