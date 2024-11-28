using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjRGsystem.Attributes;
using prjRGsystem.Context;
using prjRGsystem.Extensions;
using prjRGsystem.Models.DbModels;
using prjRGsystem.Models.ViewModels;
using prjRGsystem.Services.DbServices;

namespace prjRGsystem.Areas.Admin.Controllers
{
    [Area(areaName: "Admin"), AppController("首頁")]
    public class DashboardController : Controller
    {
        private readonly ISession? session;
        private readonly ILogger<DashboardController> logger;
        private readonly FixItemsService fixItemsService;
        private readonly SysUserService sysUserService;
        private readonly OutMoneyService outMoneyService;
        public DashboardController(IHttpContextAccessor _httpContextAccessor,
            ILogger<DashboardController> _logger,
            FixItemsService _fixItemsService,
            SysUserService _sysUserService,
            OutMoneyService _outMoneyService)
        {
            if (_httpContextAccessor.HttpContext is not null)
                session = _httpContextAccessor.HttpContext.Session;
            logger = _logger;
            fixItemsService = _fixItemsService;
            sysUserService = _sysUserService;
            outMoneyService = _outMoneyService;
        }
        public async Task<IActionResult> Index()
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel();
            dashboardViewModel.toDayFixItems = fixItemsService.ExcuteQuery().ToList();
            await fixItemsService.PrepareDataAsync(dashboardViewModel.toDayFixItems);
            dashboardViewModel.dayTotal = dashboardViewModel.toDayFixItems.Sum(n => n.total);
            List<SysUser> sysUsers = await sysUserService.GetEntitiesQ().ToListAsync();
            await sysUserService.PrepareDataAsync(sysUsers);
            dashboardViewModel.sysUsers = sysUsers;
            dashboardViewModel.outMoney = await outMoneyService.GetEntitiesQ().Where(n => n.createDate.Value.Date == DateTime.Now.Date).ToListAsync();
            dashboardViewModel.outMoneyTotal = outMoneyService.GetEntitiesQ().Where(n => n.createDate.Value.Date == DateTime.Now.Date).Sum(n => n.itemMoney);
            return View(dashboardViewModel);
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
