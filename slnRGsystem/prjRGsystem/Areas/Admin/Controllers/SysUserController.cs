using prjRGsystem.Context;
using prjRGsystem.Attributes;
using prjRGsystem.Extensions;
using prjRGsystem.Managers.DbManagers;
using prjRGsystem.Models;
using prjRGsystem.Models.DbModels;
using prjRGsystem.Services.DbServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;

namespace ICDFOverseasProperty.Areas.Admin.Controllers
{
    [Area(areaName: "Admin"), AppController("帳號管理")]
    public class SysUserController : Controller
    {
        private readonly ISession? session;
        private readonly SysUserService sysUserService;
        private readonly SysDepartmentService sysDepartmentService;
        private readonly ILogger<SysUserController> logger;
        private readonly SysUserSubstituteService sysUserSubstituteService;
        private readonly SysUserAskForLeaveService sysUserAskForLeaveService;
        public SysUserController(IHttpContextAccessor _httpContextAccessor,
            SysUserService _sysUserService,
            SysDepartmentService _sysDepartmentService,
            ILogger<SysUserController> _logger,
            SysUserSubstituteService _sysUserSubstituteService,
            SysUserAskForLeaveService _sysUserAskForLeaveService)
        {
            if (_httpContextAccessor.HttpContext is not null)
                session = _httpContextAccessor.HttpContext.Session;
            sysUserService = _sysUserService;
            sysDepartmentService = _sysDepartmentService;
            logger = _logger;
            sysUserSubstituteService = _sysUserSubstituteService;
            sysUserAskForLeaveService = _sysUserAskForLeaveService;
        }

        [AppAction("搜尋頁面", RolePrivilegeType.VIEW)]
        public async Task<IActionResult> Index(SysUserManager.Criteria criteria)
        {
            int pagesize = 100;
            int pagecurrent = criteria.pageNumber ?? 1;
            IPagedList<SysUser> pagedList = await sysUserService.ExcuteQuery(criteria).OrderBy(o => o.userId).ToPagedListAsync(pagecurrent, pagesize);
            await sysUserService.PrepareDataAsync(await pagedList.ToListAsync());
            List<SysDepartment> sysDepartment = await sysDepartmentService.GetEnterpriseEntitiesQ().ToListAsync();
            UserContext userContext = GetUserLogin() ?? new();
            SysUser sysUser = userContext.user ?? new();
            List<SysRoles> adminSysRoles = userContext != null && userContext.sysRoles != null ? userContext.sysRoles.Where(m => m.roleName != null && m.roleName.Contains("管理員")).ToList() : new List<SysRoles>();
            bool isAdmin = adminSysRoles.Count > 0 ? true : false;

            ViewData["criteria"] = criteria;
            ViewData["sysDepartment"] = sysDepartment;
            ViewData["isAdmin"] = isAdmin;

            return View(pagedList);
        }

        [HttpPost, AppAction("刪除功能", RolePrivilegeType.REMOVED)]
        public async Task<string> Removed(Int64 id)
        {
            Dictionary<String, Object> valueObject = new Dictionary<String, Object>();
            try
            {
                SysUser? editUser = await sysUserService.GetByIdAsync(id, true);
                if (editUser == null)
                {
                    valueObject.Add("success", false);
                    valueObject.Add("message", "找不到此筆資料");
                }
                else
                {
                    List<Int64> sysUserSubstituteIds = await sysUserSubstituteService.GetEnterpriseEntitiesQ().Where(m => m.sysUserId == id).Select(m => m.id).ToListAsync();
                    List<Int64> sysUserAskForLeaveIds = await sysUserAskForLeaveService.GetEnterpriseEntitiesQ().Where(m => m.sysUserId == id).Select(m => m.id).ToListAsync();
                    if (sysUserSubstituteIds != null && sysUserSubstituteIds.Count > 0)
                        await sysUserSubstituteService.RemovedAsync(sysUserSubstituteIds);
                    if (sysUserAskForLeaveIds != null && sysUserAskForLeaveIds.Count > 0)
                        await sysUserAskForLeaveService.RemovedAsync(sysUserAskForLeaveIds);
                    await sysUserService.RemovedAsync(id);
                    valueObject.Add("success", true);
                    valueObject.Add("message", "刪除成功");
                }
            }
            catch (Exception ex)
            {
                valueObject.Add("success", false);
                valueObject.Add("message", ex.Message);
                logger.LogError(ex, "message:{message}", ex.Message);
            }

            return JsonConvert.SerializeObject(valueObject);
        }

        [AppAction("編輯頁面", new[] { RolePrivilegeType.ADD, RolePrivilegeType.EDIT })]
        public async Task<IActionResult> Edit(Int64 id)
        {
            if (id != 0)
            {
                List<SysUser>? sysUsers = await sysUserService.GetEnterpriseEntitiesQ().Where(m => m.enabled == true && m.id != id).ToListAsync();
                List<long> sysDepartmentIds = sysUsers.Select(m => m.userDepartmentId).Distinct().ToList();
                Dictionary<Int64, SysDepartment> dicSysDepartments = await sysDepartmentService.GetByIds(sysDepartmentIds).Where(m => m.enabled == true).OrderBy(m => m.departmentName).ToDictionaryAsync(g => g.id, g => g);
                Dictionary<Int64, List<SysUser>> dicSysUserMap = sysUserService.DepartmentSysUserDropDownMenu(sysUsers, sysDepartmentIds, dicSysDepartments);
                ViewData["dicSysDepartments"] = dicSysDepartments;
                ViewData["dicSysUserMap"] = dicSysUserMap;
            }
            ViewData["sysDepartmentSelect"] = await sysDepartmentService.GetEnterpriseEntitiesQ().ToListAsync();
            SysUser? sysUser = await sysUserService.GetByIdAsync(id);
            if (sysUser != null)
                await sysUserService.PrepareDataAsync(sysUser);
            else
                sysUser = new SysUser();
            return View(sysUser);
        }

        [AppAction("JSON採購團使用者下拉選單")]
        public async Task<string> GetTechnicalTeamUserDropDownList(long department_id)
        {
            Dictionary<String, Object> valueObject = new Dictionary<String, Object>();
            try
            {
                List<SysDepartment> technicalTeamDepartments = await sysDepartmentService
                    .GetByBusinessTypes(new() { BusinessType.TECHNICAL_TEAM }).ToListAsync();
                List<SysDepartment> pickTechnicalTeamDepartments = technicalTeamDepartments
                    .Where(item => item.id == department_id).ToList();
                List<long> technicalTeamDepartmentIds = pickTechnicalTeamDepartments
                    .Select(item => item.id).ToList();
                List<SysUser> technicalTeamUsers = sysUserService
                    .GetByUserDepartmentIds(technicalTeamDepartmentIds).ToList();
                List<Dictionary<string, string>> dropDownList = await technicalTeamUsers
                    .Select(item => new Dictionary<string, string>()
                        {
                            { "text" , item.userName ?? "" },
                            { "value", item.id.ToString() }
                        }
                    )
                    .ToListAsync();
                valueObject.Add("success", true);
                valueObject.Add("data", dropDownList);
            }
            catch (Exception ex)
            {
                valueObject.Add("success", false);
                valueObject.Add("message", ex.Message);
                logger.LogError(ex, "message:{message}", ex.Message);
            }

            return JsonConvert.SerializeObject(valueObject);
        }

        [HttpPost, AppAction("使用者基本資料儲存功能", new[] { RolePrivilegeType.ADD, RolePrivilegeType.EDIT })]
        public async Task<string> SaveSysUser(SysUser sysUser)
        {
            Dictionary<String, Object> valueObject = new Dictionary<String, Object>();
            if (sysUser != null)
            {
                try
                {
                    string userId = sysUser.userId ?? "";
                    string email = sysUser.email ?? "";
                    bool repeatSysUsersBool = await sysUserService.GetEnterpriseEntitiesQ().AnyAsync(m => m.userId == userId && m.id != sysUser.id);
                    bool repeatSysUsersMailBool = await sysUserService.GetEnterpriseEntitiesQ().AnyAsync(m => email == m.email && m.id != sysUser.id);
                    bool repeatSysUserEmpIdBool = await sysUserService.GetEnterpriseEntitiesQ().AnyAsync(m => m.empId == sysUser.empId && m.empId != null && m.id != sysUser.id);
                    if (repeatSysUsersBool)
                    {
                        valueObject.Add("success", false);
                        valueObject.Add("message", "帳號重複，請重新輸入");
                    }
                    else if (repeatSysUsersMailBool)
                    {
                        valueObject.Add("success", false);
                        valueObject.Add("message", "Email重複，請重新輸入");
                    }
                    else if (repeatSysUserEmpIdBool)
                    {
                        valueObject.Add("success", false);
                        valueObject.Add("message", "員工編號重複，請重新輸入");
                    }
                    else
                    {
                        SysUser? editSysUsert = new SysUser();
                        if (sysUser.id > 0)
                            editSysUsert = await sysUserService.GetByIdAsync(sysUser.id, true);
                        if (editSysUsert != null)
                        {
                            string pwdNumber = "";
                            if (editSysUsert.id == 0)
                            {
                                pwdNumber = sysUserService.RandomNumber();
                                editSysUsert.password = pwdNumber;
                                editSysUsert.resetPassword = true;
                            }
                            editSysUsert.userId = sysUser.userId;
                            editSysUsert.userName = sysUser.userName;
                            editSysUsert.email = sysUser.email;
                            editSysUsert.userDepartmentId = sysUser.userDepartmentId;
                            editSysUsert.enabled = sysUser.enabled;
                            editSysUsert.empId = sysUser.empId;
                            editSysUsert.password=sysUser.password;
                            await sysUserService.SaveAsync(editSysUsert);
                            if (pwdNumber == "")
                            {
                                valueObject.Add("success", true);
                                valueObject.Add("message", "儲存成功。");
                            }
                            else
                            {
                                valueObject.Add("success", true);
                                valueObject.Add("message", "新增成功，預設密碼為" + pwdNumber);
                            }
                        }
                        else
                        {
                            valueObject.Add("success", false);
                            valueObject.Add("message", "找不到此筆資料");
                        }
                    }
                }
                catch (Exception ex)
                {
                    valueObject.Add("success", false);
                    valueObject.Add("message", ex.Message);
                    logger.LogError(ex, "message:{message}", ex.Message);
                }
            }

            return JsonConvert.SerializeObject(valueObject);
        }

        [HttpPost, AppAction("代理人儲存功能", new[] { RolePrivilegeType.ADD, RolePrivilegeType.EDIT })]
        public async Task<string> SaveSysUserSubstitute(SysUserSubstitute sysUserSubstitute)
        {
            Dictionary<String, Object> valueObject = new Dictionary<String, Object>();
            if (sysUserSubstitute != null)
            {
                try
                {
                    SysUserSubstitute? editSysUserSubstitute = new SysUserSubstitute();
                    if (sysUserSubstitute.id > 0)
                        editSysUserSubstitute = await sysUserSubstituteService.GetByIdAsync(sysUserSubstitute.id, true);
                    if (editSysUserSubstitute != null)
                    {
                        editSysUserSubstitute.sysUserId = sysUserSubstitute.sysUserId;
                        editSysUserSubstitute.substituteId = sysUserSubstitute.substituteId;
                        await sysUserSubstituteService.SaveAsync(editSysUserSubstitute);
                        valueObject.Add("success", true);
                        valueObject.Add("message", "儲存成功。");
                        valueObject.Add("successId", editSysUserSubstitute.id);
                    }
                    else
                    {
                        valueObject.Add("success", false);
                        valueObject.Add("message", "找不到此筆資料");
                    }
                }
                catch (Exception ex)
                {
                    valueObject.Add("success", false);
                    valueObject.Add("message", ex.Message);
                    logger.LogError(ex, "message:{message}", ex.Message);
                }
            }

            return JsonConvert.SerializeObject(valueObject);
        }

        [HttpPost, AppAction("使用者休假儲存功能", new[] { RolePrivilegeType.ADD, RolePrivilegeType.EDIT })]
        public async Task<string> SaveSysUserAskForLeave(SysUserAskForLeave sysUserAskForLeave)
        {
            Dictionary<String, Object> valueObject = new Dictionary<String, Object>();
            if (sysUserAskForLeave != null)
            {
                try
                {
                    SysUserAskForLeave? editSysUserAskForLeave = new SysUserAskForLeave();
                    if (sysUserAskForLeave.id > 0)
                    {
                        editSysUserAskForLeave = await sysUserAskForLeaveService.GetByIdAsync(sysUserAskForLeave.id);
                        if (editSysUserAskForLeave != null)
                        {
                            await sysUserAskForLeaveService.RemovedAsync(sysUserAskForLeave.id);
                            valueObject.Add("success", true);
                            valueObject.Add("message", "儲存成功。");
                            valueObject.Add("successId", editSysUserAskForLeave.id);
                        }
                        else
                        {
                            valueObject.Add("success", false);
                            valueObject.Add("message", "找不到此筆資料");
                        }
                    }
                    else
                    {
                        editSysUserAskForLeave.sysUserId = sysUserAskForLeave.sysUserId;
                        editSysUserAskForLeave.startLeaveDate = sysUserAskForLeave.startLeaveDate;
                        editSysUserAskForLeave.endLeaveDate = sysUserAskForLeave.endLeaveDate;
                        await sysUserAskForLeaveService.SaveAsync(editSysUserAskForLeave);
                        valueObject.Add("success", true);
                        valueObject.Add("message", "儲存成功。");
                        valueObject.Add("successId", editSysUserAskForLeave.id);
                    }
                }
                catch (Exception ex)
                {
                    valueObject.Add("success", false);
                    valueObject.Add("message", ex.Message);
                    logger.LogError(ex, "message:{message}", ex.Message);
                }
            }
            return JsonConvert.SerializeObject(valueObject);
        }

        [AppAction("瀏覽頁面", RolePrivilegeType.VIEW)]
        public async Task<IActionResult> SysUserView(Int64 id)
        {
            if (id != 0)
            {
                List<SysUser>? sysUsers = await sysUserService.GetEnterpriseEntitiesQ().Where(m => m.enabled == true && m.id != id).ToListAsync();
                List<long> sysDepartmentIds = sysUsers.Select(m => m.userDepartmentId).Distinct().ToList();
                Dictionary<Int64, SysDepartment> dicSysDepartments = await sysDepartmentService.GetByIds(sysDepartmentIds).Where(m => m.enabled == true).OrderBy(m => m.departmentName).ToDictionaryAsync(g => g.id, g => g);
                Dictionary<Int64, List<SysUser>> dicSysUserMap = sysUserService.DepartmentSysUserDropDownMenu(sysUsers, sysDepartmentIds, dicSysDepartments);
                ViewData["dicSysDepartments"] = dicSysDepartments;
                ViewData["dicSysUserMap"] = dicSysUserMap;
            }
            ViewData["sysDepartmentSelect"] = await sysDepartmentService.GetEnterpriseEntitiesQ().ToListAsync();
            SysUser? sysUser = await sysUserService.GetByIdAsync(id);
            if (sysUser != null)
                await sysUserService.PrepareDataAsync(sysUser);
            else
                sysUser = new SysUser();
            return View(sysUser);
        }
        /// <summary>
        /// 取得 UserContext 資訊 (可能需要搬移至共用)
        /// </summary>
        /// <returns></returns>
        private protected UserContext GetUserLogin()
        {
            UserContext? userContext = null;
            if (session != null)
                userContext = session.GetObjectFromJson<UserContext>(UserContext.SESSION_NAME.ToString());
            return userContext ?? new UserContext();
        }

    }
}
