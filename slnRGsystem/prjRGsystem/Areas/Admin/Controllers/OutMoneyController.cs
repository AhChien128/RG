using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using prjRGsystem.Attributes;
using prjRGsystem.Models;
using prjRGsystem.Models.DbModels;
using prjRGsystem.Services.DbServices;

namespace prjRGsystem.Areas.Admin.Controllers
{
    [Area(areaName: "Admin"), AppController("支出")]
    public class OutMoneyController : Controller
    {

        private readonly ISession? session;
        private readonly ILogger<OutMoneyController> logger;
        private readonly OutMoneyService outMoneyService;

        public OutMoneyController(IHttpContextAccessor _httpContextAccessor,
            ILogger<OutMoneyController> _logger,
             OutMoneyService _outMoneyService)
        {
            if (_httpContextAccessor.HttpContext is not null)
                session = _httpContextAccessor.HttpContext.Session;
            logger = _logger;
            outMoneyService = _outMoneyService;
        }
        public async Task<IActionResult> Edit(long id)
        {
            OutMoney? outMoney = await outMoneyService.GetByIdAsync(id) ?? new();
            return View(outMoney);
        }
        public async Task<string> Save(OutMoney putOption)
        {
            Dictionary<String, Object> valueObject = new Dictionary<String, Object>();
            try
            {
                OutMoney? outMoney = putOption.id > 0 ? await outMoneyService.GetByIdAsync(putOption.id, true) : new OutMoney();
                if (outMoney != null)
                {
                    outMoney.itemMoney = putOption.itemMoney;
                    outMoney.item = putOption.item;
                    await outMoneyService.SaveAsync(outMoney);
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
                    OutMoney? outMoney = await outMoneyService.GetByIdAsync(id.Value, true);
                    if (outMoney == null)
                    {
                        valueObject.Add("success", false);
                        valueObject.Add("message", "找不到此筆資料");
                    }
                    else
                    {
                        await outMoneyService.RemovedAsync(id.Value);
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
