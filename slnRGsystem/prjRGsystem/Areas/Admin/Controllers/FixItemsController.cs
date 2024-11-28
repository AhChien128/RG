using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using prjRGsystem.Attributes;
using prjRGsystem.Models;
using prjRGsystem.Models.DbModels;
using prjRGsystem.Models.ViewModels;
using prjRGsystem.Services.DbServices;

namespace prjRGsystem.Areas.Admin.Controllers
{
    [Area(areaName: "Admin"), AppController("今日維修項目")]
    public class FixItemsController : Controller
    {
        private readonly ISession? session;
        private readonly ILogger<FixItemsController> logger;
        private readonly MemberCarsService memberCarsService;
        private readonly MemberService memberService;
        private readonly ItemsService itemsService;
        private readonly FixItemsService fixItemsService;
        private readonly SysUserService sysUserService;
        private readonly ItemOrderService itemOrderService;
        public FixItemsController(IHttpContextAccessor _httpContextAccessor, ILogger<FixItemsController> _logger
            , MemberCarsService __memberCarsService,
            MemberService _memberService,
            ItemsService _itemsService,
            FixItemsService _fixItemsService,
            SysUserService _sysUserService,
            ItemOrderService _itemOrderService)
        {
            if (_httpContextAccessor.HttpContext is not null)
                session = _httpContextAccessor.HttpContext.Session;
            logger = _logger;
            memberCarsService = __memberCarsService;
            memberService = _memberService;
            itemsService = _itemsService;
            fixItemsService = _fixItemsService;
            sysUserService = _sysUserService;
            itemOrderService = _itemOrderService;
        }
        public async Task<IActionResult> Edit(int id)
        {
            FixItems? fixItems = await fixItemsService.GetByIdAsync(id) ?? new();
            await fixItemsService.PrepareDataAsync(fixItems);
            List<MemberCars>? memberCars = await memberCarsService.GetEntitiesQ().ToListAsync() ?? new();
            List<SysUser>? sysUsers = await sysUserService.GetEntitiesQ().ToListAsync() ?? new();
            ViewData["memberCars"] = memberCars;
            ViewData["sysUsers"] = sysUsers;
            return View(fixItems);

        }

        public async Task<string> Save(FixItems putOption)
        {
            Dictionary<String, Object> valueObject = new Dictionary<String, Object>();
            try
            {
                FixItems? fixItems = await fixItemsService.GetByIdAsync(putOption.id, true) ?? new();
                await fixItemsService.PrepareDataAsync(fixItems);
                if (fixItems != null)
                {
                    fixItems.sysUserID = putOption.sysUserID;
                    fixItems.km = putOption.km;
                    fixItems.memberCarID = putOption.memberCarID;
                    await fixItemsService.SaveAsync(fixItems);
                    List<long> itemOrderIDs = itemOrderService.GetEntitiesQ().Where(n => n.fixitemsID == fixItems.id).Select(n => n.id).ToList() ?? new();
                    await itemOrderService.RemovedAsync(itemOrderIDs);
                    if (putOption.selectedItemIDs != null)
                    {
                        for (int i = 0; i < putOption.selectedItemIDs.Count; i++)
                        {
                            ItemOrder itemOrder = new ItemOrder();
                            itemOrder.ItemID = putOption.selectedItemIDs[i];
                            itemOrder.fixitemsID = fixItems.id;
                            await itemOrderService.SaveAsync(itemOrder);
                        }
                    }
                    valueObject.Add("success", true);
                    valueObject.Add("message", "儲存成功");
                }
                else
                {
                    valueObject.Add("success", false);
                    valueObject.Add("message", "找不到此筆資料");
                }

            }
            catch (Exception ex)
            {
                valueObject.Add("success", false);
                valueObject.Add("message", ex.Message);
                logger.LogError(ex, "message:{message}", ex.Message);
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
                    FixItems? fixItems = await fixItemsService.GetByIdAsync(id.Value, true);
                    List<long> itemOrders = await itemOrderService.GetEntitiesQ().Where(n => n.fixitemsID == id).Select(n => n.id).ToListAsync();
                    if (fixItems == null)
                    {
                        valueObject.Add("success", false);
                        valueObject.Add("message", "找不到此筆資料");
                    }
                    else
                    {
                        await fixItemsService.RemovedAsync(id.Value);
                        if (itemOrders.Count > 0)
                        {
                            await itemOrderService.RemovedAsync(itemOrders);
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

        /// <summary>
        /// 維修項目連動客戶
        /// </summary>
        /// <param name="userDepartmentId"></param>
        /// <returns></returns>
        public async Task<JsonResult> LoadMemberInfo(Int64 memberCarsID)
        {
            MemberCars? memberCars = memberCarsService.GetById(memberCarsID) ?? new();
            await memberCarsService.PrepareDataAsync(memberCars);
            //Member? members = memberService.GetById(memberID) ?? new();
            return Json(memberCars);
        }


    }
}
