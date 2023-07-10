using Microsoft.AspNetCore.Mvc;
using prjRGsystem.Attributes;

namespace prjRGsystem.Areas.Admin.Views
{
    [Area(areaName: "Admin"), AppController("預約")]
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
