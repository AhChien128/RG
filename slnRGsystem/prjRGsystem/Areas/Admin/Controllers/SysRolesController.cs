using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using prjRGsystem.Attributes;
using prjRGsystem.Managers.DbManagers;
using prjRGsystem.Models.DbModels;
using prjRGsystem.Models.ViewModels;
using prjRGsystem.Services.DbServices;
using X.PagedList;
using Microsoft.EntityFrameworkCore;

namespace prjRGsystem.Areas.Admin.Controllers
{
    [Area(areaName: "Admin"), AppController("角色權限設定")]
    public class SysRolesController : Controller
    {
        private readonly SysRolesService sysRolesService;
        private readonly SysUsersRolesService sysUsersRolesService;
        private readonly SysRolesTasksService sysRolesTasksService;
        private readonly SysUserService sysUserService;
        private readonly SysTasksService sysTasksService;
        private readonly SysTasksBlackService sysTasksBlackService;
        private readonly SysRolesTasksBlackService sysRolesTasksBlackService;
        private readonly SysDepartmentService sysDepartmentService;

        private readonly ILogger<SysRolesController> logger;
        public SysRolesController(SysRolesService _sysRolesServeice,
            SysUsersRolesService _sysUsersRolesServeice,
            SysRolesTasksService _sysRolesTasksService,
            SysUserService _sysUserService,
            SysTasksService _sysTasksService,
            SysTasksBlackService _sysTasksBlackService,
            SysRolesTasksBlackService _sysRolesTasksBlackService,
            SysDepartmentService _sysDepartmentService,
            ILogger<SysRolesController> _logger)
        {
            sysRolesService = _sysRolesServeice;
            sysUsersRolesService = _sysUsersRolesServeice;
            sysRolesTasksService = _sysRolesTasksService;
            sysUserService = _sysUserService;
            sysTasksService = _sysTasksService;
            sysTasksBlackService = _sysTasksBlackService;
            sysRolesTasksBlackService = _sysRolesTasksBlackService;
            sysDepartmentService = _sysDepartmentService;
            logger = _logger;
        }

        [AppAction("角色權限設定搜尋頁面")]
        public async Task<IActionResult> Index(SysRolesManager.Criteria criteria)
        {

            int pageNumber = criteria.pageNumber ?? 1;
            int pageSize = criteria.pageSize ?? 100;
            IPagedList<SysRoles> pagedList = await sysRolesService.ExcuteQuery(criteria).OrderByDescending(m => m.createDate).ToPagedListAsync(pageNumber, pageSize);
            await sysRolesService.PrepareDataAsync(await pagedList.ToListAsync());

            ViewData["criteria"] = criteria;
            return View(pagedList);
        }

        [AppAction("角色新增/修改頁面")]
        public async Task<IActionResult> Edit(Int64? id)
        {
            SysRolesListViewModel viewModel = new SysRolesListViewModel();
            if (id != null && id > 0)
            {
                List<SysRolesTasks> sysRolesTasks = await sysRolesTasksService.GetBySysRolesId(id.Value).OrderBy(m => m.sysTasksId).ToListAsync();
                List<SysTasksBlack> sysTasksBlack = await sysTasksBlackService.GetEnterpriseEntitiesQ().OrderBy(m => m.sysTasksId).ToListAsync();
                //List<SysRolesTasksBlack> sysRolesTasksBlacks = await sysRolesTasksBlackService.GetEnterpriseEntitiesQ().ToListAsync();
                List<string> permissions = new List<string>();
                List<SysRolesTasksBlack> blackList = new List<SysRolesTasksBlack>();
                viewModel.sysRolesId = id.Value;
                viewModel.sysRole = sysRolesService.GetById(id.Value);
                viewModel.sysRolesTasks = sysRolesTasks;
                viewModel.sysUsersRoles = await sysUsersRolesService.GetBySysRoleId(id.Value).OrderBy(m => m.sysUsersId).ToListAsync();
                viewModel.sysTasksBlack = sysTasksBlack;
                List<Int64> blackforSysRolesTasksIds = sysRolesTasks.Select(m => m.id).ToList();
                List<SysRolesTasksBlack> sysRolesTasksBlacks = await sysRolesTasksBlackService.GetEnterpriseEntitiesQ().Where(m => blackforSysRolesTasksIds.Contains(m.sysRolesTasksId ?? 0)).ToListAsync();
                List<Int64> blackforIds = sysRolesTasksBlacks.Select(n => n.sysTasksBlackId ?? 0).ToList();
                List<string> hasBlackCheck = sysTasksBlack.Where(m => blackforIds.Contains(m.id)).Select(n => n.actionName).ToList();
                viewModel.sysRolesTasksBlack = sysRolesTasksBlacks;
                viewModel.hasBlackCheck = hasBlackCheck;
                if (viewModel.sysRolesTasks != null && viewModel.sysRolesTasks.Count > 0)
                {
                    foreach (SysRolesTasks sysRolesTasksper in viewModel.sysRolesTasks)
                    {
                        string dataId = sysRolesTasksper.sysTasksId.ToString();
                        if (sysRolesTasksper.isAdd == true)
                            permissions.Add(dataId + "_Add");
                        if (sysRolesTasksper.isView == true)
                            permissions.Add(dataId + "_View");
                        if (sysRolesTasksper.isEdit == true)
                            permissions.Add(dataId + "_Edit");
                        if (sysRolesTasksper.isAudit == true)
                            permissions.Add(dataId + "_Audit");
                        if (sysRolesTasksper.isRemoved == true)
                            permissions.Add(dataId + "_Delete");
                        if (sysRolesTasksper.isReport == true)
                            permissions.Add(dataId + "_Report");
                        if (sysRolesTasksper.isAdmin == true)
                            permissions.Add(dataId + "_Admin");
                    }
                }
                viewModel.permissions = permissions;
            }
            else
            {
                viewModel.sysRolesId = 0;
                viewModel.sysRole = new SysRoles();
                viewModel.sysRolesTasks = new List<SysRolesTasks>();
                viewModel.sysUsersRoles = new List<SysUsersRoles>();
            }
            Dictionary<Int64, string?> idAndDepartmentNames = sysDepartmentService.GetEntitiesQ().Where(m => m.enabled == true).ToDictionary(m => m.id, k => k.departmentName);
            ViewData["sysUsers"] = await sysUserService.GetEntitiesQ().Where(m => m.enabled == true && m.userName != "對應歷史資料用帳號").OrderBy(m => m.userDepartmentId).ToListAsync();
            ViewData["sysTasks"] = await sysTasksService.GetEntitiesQ().OrderBy(m => m.parentTasksId).ThenBy(m => m.taskSort).ToListAsync();
            List<SysTasksBlack>? sysTasksBlackList = await sysTasksBlackService.GetEnterpriseEntitiesQ().OrderBy(m => m.sysTasksId).ToListAsync();
            Dictionary<long, List<SysTasksBlack>> dicSysTasksBlack = sysTasksBlackList.GroupBy(g => g.sysTasksId).ToDictionary(g => g.Key ?? 0, g => g.ToList());
            ViewData["dicSysTasksBlack"] = dicSysTasksBlack;
            ViewData["idAndDepartmentNames"] = idAndDepartmentNames;
            ViewData["blockforSysRolesTasksIds"] = await sysRolesTasksService.GetBySysRolesId(id.Value).OrderBy(m => m.sysTasksId).Select(m => m.id).ToListAsync();
            return View(viewModel);
        }

        [HttpPost, AppAction("角色新增/修改儲存")]
        public async Task<string> Save(SysRolesListViewModel viewModel)
        {
            Dictionary<String, Object> valueObject = new Dictionary<String, Object>();
            Int64 sysRolesId = Convert.ToInt64(viewModel.sysRolesId.ToString());
            List<string> sysUserIds = !string.IsNullOrEmpty(viewModel.sysUserIds) ? viewModel.sysUserIds.Split(",").ToList() : new List<string>();
            List<string> sysTasksString = !string.IsNullOrEmpty(viewModel.sysTasksString) ? viewModel.sysTasksString.Split(",").ToList() : new List<string>();
            try
            {
                SysRoles? sysRoles = new SysRoles();
                if (sysRolesId > 0)
                {
                    sysRoles = sysRolesService.GetById(sysRolesId, true);
                    if (sysRoles != null)
                    {
                        sysRoles.roleName = !string.IsNullOrEmpty(viewModel.roleName) ? viewModel.roleName : "";
                        await sysRolesService.SaveAsync(sysRoles);
                        List<SysUsersRoles> sysUsersRoles = await sysUsersRolesService.GetBySysRoleId(sysRolesId).ToListAsync();
                        List<Int64> sysUserRolesIds = sysUsersRoles.Select(m => m.id).ToList();
                        await sysUsersRolesService.RemovedAsync(sysUserRolesIds);
                        List<SysRolesTasks> sysRolesTasks = await sysRolesTasksService.GetBySysRolesId(sysRolesId).ToListAsync();
                        List<Int64> sysRolesTasksIds = sysRolesTasks.Select(m => m.id).ToList();
                        await sysRolesTasksService.RemovedAsync(sysRolesTasksIds);
                        List<SysRolesTasksBlack> sysRolesTasksBlack = await sysRolesTasksBlackService.GetBySysRolesTasksIds(sysRolesTasksIds).ToListAsync();
                        List<Int64> sysRolesTasksBlackIds = sysRolesTasksBlack.Select(m => m.id).ToList();
                        await sysRolesTasksBlackService.RemovedAsync(sysRolesTasksBlackIds);
                    }
                    else
                    {
                        valueObject.Add("success", false);
                        valueObject.Add("message", "查無相關角色");
                        return JsonConvert.SerializeObject(valueObject);
                    }
                }
                if (sysRoles != null)
                {
                    sysRoles.roleName = !string.IsNullOrEmpty(viewModel.roleName) ? viewModel.roleName : "";
                    await sysRolesService.SaveAsync(sysRoles);
                    sysRolesId = sysRoles.id;
                }
                await sysRolesService.SaveNewSysUserRoles(sysRolesId, sysUserIds);
                await sysRolesService.HandleAndSaveSysRolesTasks(sysRolesId, sysTasksString);
                await sysRolesTasksBlackService.SaveNewBlackTasks(sysRolesId, sysTasksString);

                valueObject.Add("success", true);
                valueObject.Add("message", "儲存成功");
            }
            catch (Exception ex)
            {
                valueObject.Add("success", false);
                valueObject.Add("message", ex.Message);
                logger.LogError(ex, "message:{message}", ex.Message);
            }
            return JsonConvert.SerializeObject(valueObject);
        }

        [HttpPost, AppAction("角色刪除")]
        public async Task<string> Removed(Int64? id)
        {
            Dictionary<String, Object> valueObject = new Dictionary<String, Object>();
            if (id is not null && id > 0)
            {
                try
                {
                    SysRoles? sysRole = await sysRolesService.GetByIdAsync(id.Value, true);
                    if (sysRole == null)
                    {
                        valueObject.Add("success", false);
                        valueObject.Add("message", "找不到此筆資料");
                    }
                    else
                    {
                        await sysRolesService.RemovedAsync(id.Value);
                        List<SysUsersRoles> sysUsersRoles = await sysUsersRolesService.GetBySysRoleId(id.Value).ToListAsync();
                        List<Int64> sysUserRolesIds = sysUsersRoles.Select(m => m.id).ToList();
                        await sysUsersRolesService.RemovedAsync(sysUserRolesIds);
                        List<SysRolesTasks> sysRolesTasks = await sysRolesTasksService.GetBySysRolesId(id.Value).ToListAsync();
                        List<Int64> sysRolesTasksIds = sysRolesTasks.Select(m => m.id).ToList();
                        await sysRolesTasksService.RemovedAsync(sysRolesTasksIds);
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
            }

            return JsonConvert.SerializeObject(valueObject);
        }
    }
}
