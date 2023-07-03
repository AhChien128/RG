using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using prjRGsystem.Attributes;
using prjRGsystem.Models;
using prjRGsystem.Models.DbModels;
using prjRGsystem.Services.DbServices;

namespace prjRGsystem.Areas.Admin.Controllers
{
    [Area(areaName: "Admin"), AppController("客戶車輛")]
    public class MemberCarsController : Controller
    {
        private readonly MemberCarsService memberCarsService;
        private readonly MemberService memberService;
        private readonly ILogger<MemberCarsController> logger;
        public MemberCarsController(IHttpContextAccessor _httpContextAccessor,
            ILogger<MemberCarsController> _logger,
            MemberCarsService _memberCarsService, MemberService _memberService)
        {
            memberCarsService = _memberCarsService;
            memberService = _memberService;
            logger = _logger;
        }
        [AppAction("車輛新增/修改頁面")]
        public async Task<IActionResult> Edit(long memberID, long id)
        {
            ViewData["name"] = await memberService.GetByIdAsync(memberID) ?? new();
            MemberCars? memberCars = await memberCarsService.GetByIdAsync(id) ?? new();
            return View(memberCars);
        }
        public async Task<string> Save(MemberCars putOption)
        {
            Dictionary<String, Object> valueObject = new Dictionary<String, Object>();
            try
            {
                MemberCars? memberCars = putOption.id > 0 ? await memberCarsService.GetByIdAsync(putOption.id, true) : new MemberCars();
                if (memberCars != null)
                {
                    memberCars.memberID = putOption.memberID;
                    memberCars.brand = putOption.brand;
                    memberCars.carNumber = putOption.carNumber;
                    memberCars.carCategroy = putOption.carCategroy;
                    await memberCarsService.SaveAsync(memberCars);
                    valueObject.Add("success", true);
                    valueObject.Add("message", "儲存成功");
                    valueObject.Add("id", putOption.memberID);
                }
                else
                {
                    valueObject.Add("success", false);
                    valueObject.Add("message", "找不到此筆資料");                    
                }

            }
            catch (Exception)
            {

            }
            return JsonConvert.SerializeObject(valueObject);
        }

        public async Task<string> Removed(long? id)
        {
            Dictionary<String, Object> valueObject = new Dictionary<String, Object>();

            if (id is not null && id > 0)
            {
                try
                {
                    MemberCars? memberCars = await memberCarsService.GetByIdAsync(id.Value, true);
                    if (memberCars == null)
                    {
                        valueObject.Add("success", false);
                        valueObject.Add("message", "找不到此筆資料");
                    }
                    else
                    {
                        await memberCarsService.RemovedAsync(id.Value);
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
