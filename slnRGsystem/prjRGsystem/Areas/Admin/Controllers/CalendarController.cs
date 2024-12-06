using Microsoft.AspNetCore.Mvc;
using prjRGsystem.Attributes;
using prjRGsystem.Models;
using prjRGsystem.Models.DbModels;
using prjRGsystem.Services.DbServices;

namespace prjRGsystem.Areas.Admin.Controllers
{
    [Area(areaName: "Admin"), AppController("行事曆")]
    public class CalendarController : Controller
    {
        private readonly SysReserveService sysReserveService;
        public CalendarController(SysReserveService _sysReserveService)
        {
            sysReserveService = _sysReserveService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetReserves(DateTime strDate, DateTime endDate)
        {
            List<SysReserve> sysReserves = sysReserveService.GetStrEndDate(strDate, endDate).ToList();

            // 將資料轉換為 FullCalendar 格式
            var events = sysReserves.Select(r => new
            {
                r.id,
                title = r.subject,
                start = r.stratDate.HasValue ? r.stratDate.Value.ToString("yyyy-MM-ddTHH:mm:ss") : "",
                end = r.endDate.HasValue ? r.endDate.Value.ToString("yyyy-MM-ddTHH:mm:ss") : "",
                backgroundColor = r.LevelType_E switch
                {
                    LevelType.High => "red",
                    LevelType.Medium => "orange",
                    LevelType.Low => "green",
                    _ => "blue"
                },
                borderColor = r.LevelType_E switch
                {
                    LevelType.High => "red",
                    LevelType.Medium => "orange",
                    LevelType.Low => "green",
                    _ => "blue"
                }

            });
            return Ok(events);
        }
        public async Task<IActionResult> EditReserves(long id)
        {
            SysReserve sysReserve = await sysReserveService.GetByIdAsync(id) ?? new();
            return PartialView(sysReserve);
        }
        [HttpPost]
        public async Task<IActionResult> SaveReserves(SysReserve edit)
        {
            try
            {
                string msg = "";
                SysReserve dbReserve = await sysReserveService.GetByIdAsync(edit.id, true) ?? new();
                dbReserve.subject = edit.subject;
                dbReserve.content = edit.content;
                dbReserve.stratDate = edit.stratDate;
                dbReserve.endDate = edit.endDate;
                dbReserve.memo = edit.memo;
                dbReserve.level = edit.level;
                int count = await sysReserveService.SaveAsync(dbReserve);
                if (count > 0)
                    msg = edit.id > 0 ? "修改成功" : "新增成功";
                else
                    msg = "成功但未異動資料";
                return Ok(new { success = true, message = msg });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "儲存預約時發生錯誤", details = ex.Message });
            }
        }
        public async Task<IActionResult> DeleteReserves(int id)
        {
            string msg = "";
            try
            {
                int count = await sysReserveService.RemovedAsync(id, true);
                if (count > 0)
                    msg = "刪除成功";
                else
                    msg = "成功但未刪除任何資料";
                return Ok(new { success = true, message = msg });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "儲存預約時發生錯誤", details = ex.Message });
            }
        }
    }
}
