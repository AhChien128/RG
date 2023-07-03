using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using prjRGsystem.Attributes;
using prjRGsystem.Models;
using prjRGsystem.Models.DbModels;
using prjRGsystem.Services.DbServices;

namespace prjRGsystem.Areas.Admin.Controllers
{
    [Area(areaName: "Admin"), AppController("客戶維護")]

    public class MaintainClientController : Controller
    {
        private MemberService memberService;
        private MemberCarsService memberCarsService;
        private ILogger<MaintainClientController> logger;
        public MaintainClientController(IHttpContextAccessor _httpContextAccessor, ILogger<MaintainClientController> _logger, MemberService _memberService, MemberCarsService _memberCarsService)
        {
            memberService = _memberService;
            logger = _logger;
            memberCarsService = _memberCarsService;
        }
        public async Task<IActionResult> Index()
        {
            List<Member> members = await memberService.GetEntitiesQ().ToListAsync();
            return View(members);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Member? member = await memberService.GetByIdAsync(id, false) ?? new();
            await memberService.PrepareDataAsync(member);
            return View(member);
        }
        public async Task<string> Save(Member putOption)
        {
            Dictionary<String, Object> valueObject = new Dictionary<String, Object>();
            try
            {
                Member? member = putOption.id > 0 ? await memberService.GetByIdAsync(putOption.id, true) : new Member();
                if (member != null)
                {
                    member.name = putOption.name;
                    member.phoneNumber = putOption.phoneNumber;
                    await memberService.SaveAsync(member);
                    valueObject.Add("success", true);
                    valueObject.Add("message", "儲存成功");
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

        [HttpPost, AppAction("刪除功能", RolePrivilegeType.REMOVED)]
        public async Task<string> Removed(long? id)
        {
            Dictionary<String, Object> valueObject = new Dictionary<String, Object>();

            if (id is not null && id > 0)
            {
                try
                {
                    Member? member = await memberService.GetByIdAsync(id.Value, true);
                    if (member == null)
                    {
                        valueObject.Add("success", false);
                        valueObject.Add("message", "找不到此筆資料");
                    }
                    else
                    {
                        await memberService.RemovedAsync(id.Value);
                        List<MemberCars> memberCars = await memberCarsService.GetMemberID(id.Value).ToListAsync();
                        foreach (MemberCars memberCar in memberCars)
                        {
                            await memberCarsService.RemovedAsync(memberCar.id);
                        }
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
