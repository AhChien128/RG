using prjRGsystem.Context;
using prjRGsystem.Managers.DbManagers;
using prjRGsystem.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace prjRGsystem.Services.DbServices
{
    public class SysUserService : SysUserManager
    {
        private readonly SysDepartmentManager sysDepartmentManager;
        private readonly SysRolesManager sysRolesManager;
        private readonly SysUsersRolesManager sysUsersRolesManager;
        private readonly SysUserSubstituteManager sysUserSubstituteManager;
        private readonly SysUserAskForLeaveManager sysUserAskForLeaveManager;
        public SysUserService(RGPropertyContext _db,
            IHttpContextAccessor _httpContextAccessor,
            SysDepartmentManager _sysDepartmentManager,
            SysRolesManager _sysRolesManager,
            SysUsersRolesManager _sysUsersRolesManager,
            SysUserSubstituteManager _sysUserSubstituteManager,
            SysUserAskForLeaveManager _sysUserAskForLeaveManager) : base(_db, _httpContextAccessor)
        {
            sysDepartmentManager = _sysDepartmentManager;
            sysRolesManager = _sysRolesManager;
            sysUsersRolesManager = _sysUsersRolesManager;
            sysUserSubstituteManager = _sysUserSubstituteManager;
            sysUserAskForLeaveManager = _sysUserAskForLeaveManager;
        }

        public async Task PrepareDataAsync(SysUser sysUser)
        {
            await PrepareDataAsync(new List<SysUser>() { sysUser });
        }

        public async Task PrepareDataAsync(List<SysUser> sysUsers)
        {
            if (sysUsers != null && sysUsers.Count > 0)
            {
                DateTime nowTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                DateTime twoWeeksTime = Convert.ToDateTime(DateTime.Now.AddDays(14).ToString("yyyy-MM-dd"));
                List<Int64> userDepartmentIds = sysUsers.Select(m => m.userDepartmentId).ToList();
                Dictionary<Int64, SysDepartment> sysDepartmentMap = await sysDepartmentManager.GetByIds(userDepartmentIds).ToDictionaryAsync(g => g.id, g => g);
                Dictionary<Int64, SysRoles> dicSysRoles = sysRolesManager.GetEntitiesQ().ToDictionary(m => m.id, m => m);
                List<SysUsersRoles> sysUsersRoles = sysUsersRolesManager.GetEntitiesQ().ToList();
                Dictionary<Int64, List<SysUsersRoles>> dicSysUsersRoles = sysUsersRoles.GroupBy(m => m.sysUsersId).ToDictionary(m => m.Key, m => m.ToList());
                List<Int64> susUserIds = sysUsers.Select(m => m.id).ToList();
                List<SysUserSubstitute> substitutes = await sysUserSubstituteManager.GetEnterpriseEntitiesQ().Where(m => susUserIds.Contains(m.sysUserId)).ToListAsync();
                Dictionary<Int64, List<SysUserSubstitute>> sysUserSubstitute = substitutes.GroupBy(m => m.sysUserId).ToDictionary(m => m.Key, m => m.ToList());
                Dictionary<Int64, SysUser> dicSysUser = await GetEnterpriseEntitiesQ().ToDictionaryAsync(m => m.id, m => m);
                List<SysUserAskForLeave> askForLeaves = await sysUserAskForLeaveManager.GetEnterpriseEntitiesQ().Where(m => susUserIds.Contains(m.sysUserId)).ToListAsync();
                Dictionary<Int64, List<SysUserAskForLeave>> sysUserAskForLeaves = askForLeaves.GroupBy(m => m.sysUserId).ToDictionary(m => m.Key, m => m.ToList());
                foreach (SysUser sysUser in sysUsers)
                {
                    if (sysDepartmentMap.ContainsKey(sysUser.userDepartmentId))
                        sysUser.userDepartment = sysDepartmentMap[sysUser.userDepartmentId];
                    if (sysUserSubstitute.ContainsKey(sysUser.id))
                    {
                        sysUser.sysUserSubstitute = new List<SysUserSubstitute>();
                        for (int i = 0; i < sysUserSubstitute[sysUser.id].Count; i++)
                        {
                            if (dicSysUser.ContainsKey(sysUserSubstitute[sysUser.id][i].substituteId))
                                sysUserSubstitute[sysUser.id][i].sysUser = dicSysUser[sysUserSubstitute[sysUser.id][i].substituteId];
                            sysUser.sysUserSubstitute.Add(sysUserSubstitute[sysUser.id][i]);
                        }
                    }
                    if (sysUserAskForLeaves != null && sysUserAskForLeaves.ContainsKey(sysUser.id))
                    {
                        sysUser.sysUserAskForLeave = new List<SysUserAskForLeave>();
                        for (int i = 0; i < sysUserAskForLeaves[sysUser.id].Count; i++)
                        {
                            sysUser.sysUserAskForLeave.Add(sysUserAskForLeaves[sysUser.id][i]);
                        }
                        SysUserAskForLeave? sysUserAskForLeave = sysUserAskForLeaves[sysUser.id].Where(m => m.startLeaveDate >= nowTime && m.startLeaveDate <= twoWeeksTime).OrderBy(m => m.startLeaveDate).FirstOrDefault();
                        if (sysUserAskForLeave != null)
                            sysUser.leaveDate = sysUserAskForLeave.startLeaveDate;
                    }
                    if (dicSysUsersRoles.ContainsKey(sysUser.id))
                    {
                        sysUser.sysRoles = new List<SysRoles>();
                        for (int i = 0; i < dicSysUsersRoles[sysUser.id].Count; i++)
                        {
                            if (dicSysRoles.ContainsKey(dicSysUsersRoles[sysUser.id][i].sysRolesId))
                                sysUser.sysRoles.Add(dicSysRoles[dicSysUsersRoles[sysUser.id][i].sysRolesId]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 部門人員下拉選單
        /// </summary>
        /// <param name="sysUsers"></param>
        /// <returns></returns>
        public Dictionary<Int64, List<SysUser>> DepartmentSysUserDropDownMenu(List<SysUser> sysUsers, List<long> sysDepartmentIds, Dictionary<Int64, SysDepartment> dicSysDepartments)
        {
            List<long> noHaveDepartmentIds = sysDepartmentIds.Where(c => !dicSysDepartments.ContainsKey(c)).ToList();
            List<SysUser> noHaveDepartmentSysUsers = sysUsers.Where(m => noHaveDepartmentIds.Contains(m.userDepartmentId)).ToList();
            Dictionary<Int64, List<SysUser>> dicSysUserMap = new();
            foreach (var id in dicSysDepartments.Keys)
            {
                List<SysUser> sysUsersforDepartmentId = sysUsers.Where(m => m.userDepartmentId == id).ToList();
                dicSysUserMap.Add(id, sysUsersforDepartmentId);
            }
            if (noHaveDepartmentSysUsers != null && noHaveDepartmentSysUsers.Count > 0)
                dicSysUserMap.Add(0, noHaveDepartmentSysUsers);
            return dicSysUserMap;
        }

        /// <summary>
        /// 產生亂數密碼
        /// </summary>
        /// <returns></returns>
        public string RandomNumber()
        {
            string pwdNumber = "";
            Random rd = new Random();
            for (int i = 0; i < 4; i++)
            {
                pwdNumber += rd.Next(0, 10).ToString(); ;
            }
            return pwdNumber;
        }

    }
}
