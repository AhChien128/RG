using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using prjRGsystem.Attributes;
using prjRGsystem.Models;
using prjRGsystem.Models.DbModels;
using prjRGsystem.Services.DbServices;
using X.PagedList;
using static prjRGsystem.Manager.DbManager.ItemsManager;

namespace prjRGsystem.Areas.Admin.Controllers
{
    [Area(areaName: "Admin"), AppController("材料維護")]
    public class ItemsController : Controller
    {
        private readonly ItemsService itemsService;
        private ILogger<ItemsController> logger;

        public ItemsController(IHttpContextAccessor _httpContextAccessor, ILogger<ItemsController> _logger, ItemsService _itemsService)
        {
            itemsService = _itemsService;
            logger = _logger;
        }
        public async Task<IActionResult> Index(Criteria criteria)
        {
            int pageSize = 1;
            int pageNumber = criteria.pageNumber < 1 ? 1 : criteria.pageNumber;
            IPagedList<Items> itemsPL = await itemsService.ExcuteQuery(criteria).ToPagedListAsync(pageNumber, pageSize);
            ViewData["criteria"] = criteria;
            return View(itemsPL);
        }
        public async Task<IActionResult> Edit(long id)
        {
            Items items = await itemsService.GetByIdAsync(id, false) ?? new();
            return View(items);
        }
        public async Task<string> Save(Items putOption)
        {
            Dictionary<String, Object> valueObject = new Dictionary<String, Object>();
            try
            {
                Items? items = putOption.id > 0 ? await itemsService.GetByIdAsync(putOption.id, true) : new Items();
                if (items != null)
                {
                    items.productBrand = putOption.productBrand;
                    items.productName = putOption.productName;
                    items.productPrice = putOption.productPrice;
                    items.productCost = putOption.productCost;
                    items.productDescribe = putOption.productDescribe;
                    await itemsService.SaveAsync(items);
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
                    Items? items = await itemsService.GetByIdAsync(id.Value, true);
                    if (items == null)
                    {
                        valueObject.Add("success", false);
                        valueObject.Add("message", "找不到此筆資料");
                    }
                    else
                    {
                        await itemsService.RemovedAsync(id.Value);
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
