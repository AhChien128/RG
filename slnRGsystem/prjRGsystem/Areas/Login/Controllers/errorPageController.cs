using Microsoft.AspNetCore.Mvc;
using prjRGsystem.Attributes;

namespace prjRGsystem.Areas.Login.Controllers
{
    [Area(areaName: "Login"), AppController("錯誤頁面")]
    public class errorPageController : Controller
    {
        [IgnoreUserLoginFilterAttribute, AppAction("錯誤頁面")]
        public IActionResult ErrorPage404()
        {
            return View();
        }

        [IgnoreUserLoginFilterAttribute, AppAction("錯誤頁面")]
        public IActionResult ErrorPage500()
        {
            return View();
        }
    }
}
