using Microsoft.AspNetCore.Mvc;
using prjRGsystem.Attributes;

namespace prjRGsystem.Areas.Admin.Controllers
{
    [Area(areaName: "Admin"), AppController("估價單")]
    public class EstimateController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
