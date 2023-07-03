using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Managers.DbManagers;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Services.DbServices
{
    public class SysRolesService : SysRolesManager
    {
        public readonly SysUsersRolesService sysUsersRolesService;
        public readonly SysRolesTasksService sysRolesTasksService;
        public readonly SysUserService sysUserService;
        public SysRolesService(RGPropertyContext _db,
            IHttpContextAccessor _httpContextAccessor,
            SysUsersRolesService _sysUsersRolesService,
            SysRolesTasksService _sysRolesTasksService,
            SysUserService _sysUserService
            ) : base(_db, _httpContextAccessor)
        {
            sysUsersRolesService = _sysUsersRolesService;
            sysRolesTasksService = _sysRolesTasksService;
            sysUserService = _sysUserService;
        }

        public async Task PrepareDataAsync(SysRoles sysRole)
        {
            await PrepareDataAsync(new List<SysRoles>() { sysRole });
        }

        public async Task PrepareDataAsync(List<SysRoles> sysRoles)
        {
            if (sysRoles != null && sysRoles.Count > 0)
            {
                List<Int64> ids = sysRoles.Select(m => m.id).ToList();
                List<SysUsersRoles> sysUsersRoles = await sysUsersRolesService.GetBySysRoleIds(ids).ToListAsync();
                Dictionary<Int64, List<SysUsersRoles>> roleIdAndSysUsersRolessMap = new Dictionary<Int64, List<SysUsersRoles>>();
                Dictionary<Int64, SysUser> idAndsysUsersMap = new Dictionary<Int64, SysUser>();

                if (sysUsersRoles != null && sysUsersRoles.Count > 0)
                {
                    roleIdAndSysUsersRolessMap = sysUsersRoles.GroupBy(m => m.sysRolesId).ToDictionary(m => m.Key, k => k.ToList());
                    List<Int64> userIds = new List<Int64>();
                    userIds = sysUsersRoles.Select(m => m.sysUsersId).ToList();
                    List<SysUser> sysUsers = await sysUserService.GetByIds(userIds).ToListAsync();
                    if (sysUsers != null && sysUsers.Count > 0)
                        idAndsysUsersMap = sysUsers.ToDictionary(m => m.id, k => k);
                }
                foreach (SysRoles role in sysRoles)
                {
                    if (roleIdAndSysUsersRolessMap.ContainsKey(role.id))
                    {
                        List<SysUsersRoles> sysUsersRolesLists = roleIdAndSysUsersRolessMap[role.id];
                        List<SysUser> sysUsers = new List<SysUser>();
                        foreach (SysUsersRoles sysUsersRole in sysUsersRolesLists)
                        {
                            SysUser? sysUser = await sysUserService.GetByIdAsync(sysUsersRole.sysUsersId);
                            if (sysUser != null && !sysUsers.Contains(sysUser))
                                if (sysUser.enabled == true)
                                    sysUsers.Add(sysUser);
                        }
                        role.sysUsers = sysUsers;
                    }
                }
            }
        }

        public async Task SaveNewSysUserRoles(Int64 sysRolesId, List<string> sysUserIds)
        {
            List<SysUsersRoles> sysUsersRolesList = new List<SysUsersRoles>();
            foreach (string userid in sysUserIds)
            {
                Int64 sysUserId = 0;
                if (Int64.TryParse(userid, out sysUserId))
                {
                    SysUsersRoles sysUsersRoles = new SysUsersRoles();
                    sysUsersRoles.id = 0;
                    sysUsersRoles.sysRolesId = sysRolesId;
                    sysUsersRoles.sysUsersId = sysUserId;
                    sysUsersRolesList.Add(sysUsersRoles);
                }
            }
            await sysUsersRolesService.SaveAsync(sysUsersRolesList);
        }

        public async Task HandleAndSaveSysRolesTasks(Int64 sysRolesId, List<string> sysTasksString)
        {
            List<SysRolesTasks> sysRolesTasksList = new List<SysRolesTasks>();
            List<IdAndPermissions> idAndPermissions = HandlePermissions(sysTasksString);

            if (idAndPermissions.Count > 0)
            {
                Int64 taskId = 0;
                SysRolesTasks sysRolesTasks = new SysRolesTasks();
                foreach (IdAndPermissions data in idAndPermissions)
                {
                    if (!taskId.Equals(data.id) && sysRolesTasks.sysTasksId > 0)
                    {
                        sysRolesTasks.sysRolesId = sysRolesId;
                        sysRolesTasksList.Add(sysRolesTasks);
                        sysRolesTasks = new SysRolesTasks();
                    }
                    sysRolesTasks.sysTasksId = data.id;
                    switch (data.permission)
                    {
                        case "Add":
                            sysRolesTasks.isAdd = true;
                            break;

                        case "View":
                            sysRolesTasks.isView = true;
                            break;

                        case "Edit":
                            sysRolesTasks.isEdit = true;
                            break;

                        case "Delete":
                            sysRolesTasks.isRemoved = true;
                            break;

                        case "Audit":
                            sysRolesTasks.isAudit = true;
                            break;
                        case "Report":
                            sysRolesTasks.isReport = true;
                            break;
                        case "Admin":
                            sysRolesTasks.isAdmin = true;
                            break;
                        default:
                            break;
                    }
                    taskId = data.id;
                }
                if (sysRolesTasks.sysTasksId > 0)
                {
                    sysRolesTasks.sysRolesId = sysRolesId;
                    sysRolesTasksList.Add(sysRolesTasks);
                }
            }
            await sysRolesTasksService.SaveAsync(sysRolesTasksList);

        }



        public class IdAndPermissions
        {
            public Int64 id { get; set; }
            public string? permission { get; set; }
        }
    }
}
