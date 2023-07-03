using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjRGsystem.Attributes;
using prjRGsystem.Context;
using prjRGsystem.Extensions;
using prjRGsystem.Models.DbModels;
using prjRGsystem.Models.ViewModels;
using prjRGsystem.Services.DbServices;
using prjRGsystem.Util;

namespace prjRGsystem.Areas.Login.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ISession? session;
        protected readonly IHttpContextAccessor httpContextAccessor;
        protected readonly SysUserService sysUserService;
        protected readonly SysUsersRolesService sysUsersRolesService;
        protected readonly SysRolesService sysRolesService;
        protected readonly SysRolesTasksService sysRolesTasksService;
        protected readonly SysTasksService sysTasksService;
        protected readonly SysDepartmentService sysDepartmentService;
        protected readonly SysTasksBlackService sysTasksBlackService;
        protected readonly SysRolesTasksBlackService sysRolesTasksBlackService;
        public BaseController(IHttpContextAccessor _httpContextAccessor,
            SysUserService _sysUserService,
            SysUsersRolesService _sysUsersRolesService,
            SysRolesService _sysRolesService,
            SysRolesTasksService _sysRolesTasksService,
            SysTasksService _sysTasksService,
            SysDepartmentService _sysDepartmentService,
            SysTasksBlackService _sysTasksBlackService,
            SysRolesTasksBlackService _sysRolesTasksBlackService
            )
        {
            if (_httpContextAccessor.HttpContext is not null)
                session = _httpContextAccessor.HttpContext.Session;
            httpContextAccessor = _httpContextAccessor;
            sysUserService = _sysUserService;
            sysUsersRolesService = _sysUsersRolesService;
            sysRolesService = _sysRolesService;
            sysRolesTasksService = _sysRolesTasksService;
            sysTasksService = _sysTasksService;
            sysDepartmentService = _sysDepartmentService;
            sysTasksBlackService = _sysTasksBlackService;
            sysRolesTasksBlackService = _sysRolesTasksBlackService;
        }
        [IgnoreUserLoginFilterAttribute, AppAction("創建UserContext功能")]
        protected async Task CreateUserContextSessionAsync(SysUser sysUser)
        {
            UserContext? userContext = (session != null) ? session.GetObjectFromJson<UserContext>(UserContext.SESSION_NAME.ToString()) : new UserContext();
            List<long> roleIds = new List<long>();//使用者角色
            List<SysUsersRoles> roles = await sysUsersRolesService.GetByUserIdAsync(sysUser.id, false);
            List<SysRoles> checkRoles = await sysRolesService.GetByIds(roles.Select(p => p.sysRolesId).ToList()).ToListAsync();
            if (roles != null && roles.Count > 0)
            {
                roleIds = roles.Where(p => checkRoles.Any(x => x.id.Equals(p.sysRolesId))).Select(m => m.sysRolesId).ToList();
            }
            List<SysRolesTasks> sysRolesTasksList = await sysRolesTasksService.GetByRolesIdsAsync(roleIds, false);
            List<SysTasks> sysTasksList = await sysTasksService.GetUserMenuAsync(sysRolesTasksList);
            TreeNode<SysTasks>? menu = TreeNodeUtil.TransformToTree<SysTasks>(sysTasksList);
            await sysUserService.PrepareDataAsync(sysUser);
            if (userContext != null)
            {
                userContext.sysRoles = checkRoles;
                userContext.user = sysUser;
                userContext.department = await sysDepartmentService.GetByIdAsync(sysUser.userDepartmentId, false);
                userContext.authorizedSysTasks = sysTasksService.GetMenus(menu);
                userContext.authorizedSysCodes = GetAuthorizedSysCodes(sysTasksList, sysRolesTasksList);
                userContext.authorizedTaskBlackNames = GetAuthorizedTaskBlackNames(sysTasksList, sysRolesTasksList);
                if (session != null)
                    session.SetObjectAsJson(UserContext.SESSION_NAME.ToString(), userContext);
            }



        }

        private Dictionary<string, List<string>> GetAuthorizedTaskBlackNames(List<SysTasks> sysTasksList, List<SysRolesTasks> sysRolesTasksList)
        {
            List<Int64> sysRolesTasksIds = sysRolesTasksList
                .Select(item => item.id)
                .ToList();
            List<SysRolesTasksBlack> sysRolesTasksBlacks = sysRolesTasksBlackService
                .GetBySysRolesTasksIds(sysRolesTasksIds)
                .ToList();
            List<Int64> sysTasksBlackIds = sysRolesTasksBlacks
                .Select(item => item.sysTasksBlackId ?? 0)
                .Distinct()
                .ToList();
            List<SysTasksBlack> sysTasksBlacks = sysTasksBlackService
                .GetByIds(sysTasksBlackIds)
                .ToList();
            Dictionary<Int64, List<string>> actionNamesMapping = sysTasksBlacks
                .GroupBy(item => item.sysTasksId ?? 0)
                .ToDictionary(item => item.Key, item => item
                    .Select(gItem => gItem.actionName ?? "")
                    .ToList()
                );
            Dictionary<string, List<string>> actionNamesByTaskNameMapping = sysTasksList.Aggregate(new Dictionary<string, List<string>>(), (prev, sysTasks) =>
            {
                List<string> actionNames = actionNamesMapping.TryGetValue(sysTasks.id, out var actionNamesValue) ? actionNamesValue : new();
                string taskName = $"{sysTasks.areaName}{sysTasks.controllerName}";
                if (!prev.ContainsKey(taskName))
                {
                    prev.Add(taskName, actionNames);
                }
                return prev;
            });
            return actionNamesByTaskNameMapping;
        }

        [IgnoreUserLoginFilterAttribute, AppAction("取得授權功能最大權限")]
        protected Dictionary<string, SysRolesTasks> GetAuthorizedSysCodes(List<SysTasks> sysTasksList, List<SysRolesTasks> sysRolesTasksList)
        {
            Dictionary<string, SysRolesTasks> rolesTasksMap = new Dictionary<string, SysRolesTasks>();
            IEnumerable<IGrouping<long, SysRolesTasks>> sysRolesTasksGroup = sysRolesTasksList.GroupBy(x => x.sysTasksId);
            Dictionary<long, string> sysTaskIdByNameMap = sysTasksList.ToDictionary(m => m.id, k => k.areaName + k.controllerName);
            foreach (IGrouping<long, SysRolesTasks> group in sysRolesTasksGroup)
            {
                SysRolesTasks authorizedSysRolesTasks = new SysRolesTasks
                {
                    //用使用者擁有角色比對，找出這個功能的最大權限
                    isAdd = group.Select(x => x.isAdd).Contains(true),// ? true : false
                    isView = group.Select(x => x.isView).Contains(true),// ? true : false
                    isEdit = group.Select(x => x.isEdit).Contains(true),// ? true : false
                    isRemoved = group.Select(x => x.isRemoved).Contains(true),// ? true : false
                    isReport = group.Select(x => x.isReport).Contains(true),// ? true : false
                    isAudit = group.Select(x => x.isAudit).Contains(true),// ? true : false
                    isAdmin = group.Select(x => x.isAdmin).Contains(true)// ? true : false
                }; //存最大權限

                string taskName = sysTaskIdByNameMap[group.Key];
                if (!rolesTasksMap.ContainsKey(taskName))
                    rolesTasksMap.Add(taskName, authorizedSysRolesTasks);

            }
            return rolesTasksMap;
        }
    }
}
