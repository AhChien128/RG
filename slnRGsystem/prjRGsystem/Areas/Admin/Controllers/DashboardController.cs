using Microsoft.AspNetCore.Mvc;
using prjRGsystem.Attributes;
using prjRGsystem.Context;
using prjRGsystem.Extensions;
using prjRGsystem.Models.DbModels;
using prjRGsystem.Services.DbServices;

namespace prjRGsystem.Areas.Admin.Controllers
{
    [Area(areaName: "Admin"), AppController("首頁")]
    public class DashboardController : Controller
    {
        private readonly ISession? session;
        private readonly ILogger<DashboardController> logger;
        private readonly FixItemsService fixItemsService;
        public DashboardController(IHttpContextAccessor _httpContextAccessor, ILogger<DashboardController> _logger, FixItemsService _fixItemsService)
        {
            if (_httpContextAccessor.HttpContext is not null)
                session = _httpContextAccessor.HttpContext.Session;
            logger = _logger;
            fixItemsService = _fixItemsService;
        }
        public IActionResult Index()
        {
            UserContext userContext = GetUserLogin() ?? new();
            SysUser sysUser = userContext.user ?? new();
            List<FixItems> fixItems = fixItemsService.ExcuteQuery().ToList();
            ViewData["sysUser"] = sysUser;

            return View(fixItems);
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
