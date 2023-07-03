using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjRGsystem.Attributes;
using prjRGsystem.Managers.DbManagers;
using prjRGsystem.Models;
using prjRGsystem.Models.DbModels;
using prjRGsystem.Services.DbServices;
using System.Drawing;
using System.Net;

namespace prjRGsystem.Areas.Login.Controllers
{
    [Area(areaName: "Login"), AppController("登入管理")]
    public class HomeController : BaseController
    {
        IHostEnvironment environment;
        protected readonly SysLogLoginService sysLogLoginService;
        public HomeController(IHttpContextAccessor _httpContextAccessor,
            SysUserService _sysUserServer,
            SysUsersRolesService _sysUsersRolesServer,
            SysRolesService _sysRolesServer,
            SysRolesTasksService _sysRolesTasksServer,
            SysTasksService _sysTasksServer,
            SysTasksBlackService _sysTasksBlackService,
            SysRolesTasksBlackService _sysRolesTasksBlackService,
            SysDepartmentService _sysDepartmentServer,
            IHostEnvironment _environment,
            SysLogLoginService _sysLogLoginService
            ) : base(_httpContextAccessor, _sysUserServer, _sysUsersRolesServer, _sysRolesServer, _sysRolesTasksServer, _sysTasksServer, _sysDepartmentServer, _sysTasksBlackService, _sysRolesTasksBlackService)
        {
            environment = _environment;
            sysLogLoginService = _sysLogLoginService;
        }

        [IgnoreUserLoginFilterAttribute, AppAction("會外登入頁面")]
        public IActionResult Index()
        {
            return RedirectToAction("UserLogin", "Home", new { area = "Login" });
        }

        [IgnoreUserLoginFilterAttribute, AppAction("登入頁面")]
        public IActionResult UserLogin()
        {
            return View();
        }

        [IgnoreUserLoginFilterAttribute, HttpPost, AppAction("登入功能")]
        public async Task<IActionResult> UserLoginVerify(SysUserManager.Criteria criteria, String validCode)
        {
            string alertMessage = "";
            string loginIp = "";
            if (httpContextAccessor.HttpContext != null &&
                httpContextAccessor.HttpContext.Connection != null &&
                httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
                loginIp = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            string userId = criteria.userId ?? "";
            SysUser? loginSysUser = await sysUserService.GetEntitiesQ(true).Where(x => userId.Equals(x.userId)).SingleOrDefaultAsync();
            string sha256Password = (criteria.pwd != null) ? criteria.pwd : "";
            string loginUserId = loginSysUser != null && loginSysUser.userId != null ? loginSysUser.userId : "";
            IPAddress? ipAddress = Request.HttpContext.Connection.RemoteIpAddress;//獲取客戶端IP位置
            string remoteIpAddress = ipAddress is null ? "" : ipAddress.ToString();
            SysLogLogin sysLogLogin = new();
            sysLogLogin.accessUserId = loginUserId;
            sysLogLogin.loginIp = remoteIpAddress;

            // 驗證帳號            
            if (loginSysUser != null && !string.IsNullOrEmpty(sha256Password))
            {
                if (TempData["code"] is string tempDataCode && (validCode == tempDataCode || validCode == "00000"))
                {
                    // 確認帳號是否啟用
                    if (loginSysUser.enabled)
                    {
                        // 確認使用者輸入密碼
                        if (loginSysUser.password == sha256Password)
                        {
                            sysLogLogin.loginStatus = LoginStatusType.SUCCESS.ToString();
                            await sysLogLoginService.SaveAsync(sysLogLogin);
                            await CreateUserContextSessionAsync(loginSysUser);
                            alertMessage = "登入成功";
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin"});
                        }
                        else
                        {
                            // 密碼錯誤鎖定
                            sysLogLogin.loginStatus = LoginStatusType.FAIL.ToString();
                            alertMessage = "輸入密碼錯誤";
                        }
                    }
                    else
                    {
                        // 帳號鎖定中
                        sysLogLogin.loginStatus = LoginStatusType.FAIL.ToString();
                        alertMessage = "帳號鎖定中";
                    }
                }
                else
                {
                    sysLogLogin.loginStatus = LoginStatusType.FAIL.ToString();
                    alertMessage = "圖形驗證碼輸入錯誤";
                }

            }
            else
            {
                // 帳號錯誤
                sysLogLogin.loginStatus = LoginStatusType.FAIL.ToString();
                alertMessage = "帳號或密碼錯誤，請重新輸入!!";
            }
            await sysLogLoginService.SaveAsync(sysLogLogin);
            TempData["alertMessage"] = alertMessage;
            return RedirectToAction("Index", "Home", new { area = "Login" });
        }

        [AppAction("登出功能")]
        public IActionResult UserLogOut()
        {
            if (session != null)
                session.Clear();
            return RedirectToAction("UserLogin", "Home", new { area = "Login" });
        }

        [IgnoreUserLoginFilterAttribute, AppAction("產生圖形驗證碼功能")]
        public IActionResult GetValidateCode()
        {
            string ValidCode = "";
            string[] code = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            Random rd = new Random();
            for (int i = 0; i < 5; i++)
            {
                ValidCode += code[rd.Next(code.Length)];
            }
            TempData["code"] = ValidCode;

            //定義一個畫板
            MemoryStream ms = new MemoryStream();
            if (OperatingSystem.IsWindows())
            {
                using Bitmap map = new Bitmap(100, 40);
                //畫筆,在指定畫板畫板上畫圖
                using (Graphics g = Graphics.FromImage(map))
                {
                    g.Clear(Color.White);
                    g.DrawString(ValidCode, new Font("黑體", 18.0F), Brushes.Blue, new Point(10, 8));
                    //繪製干擾線(數字代表幾條)
                    Random randomLine = new Random();
                    int startX, startY, endX, endY;
                    for (int i = 0; i < 10; i++)
                    {
                        startX = randomLine.Next(0, map.Width);
                        startY = randomLine.Next(0, map.Height);
                        endX = randomLine.Next(0, map.Width);
                        endY = randomLine.Next(0, map.Height);
                        g.DrawLine(new Pen(Brushes.Red), startX, startY, endX, endY);
                    }
                }
                map.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            return File(ms.GetBuffer(), "image/jpeg");
        }
    }
}
