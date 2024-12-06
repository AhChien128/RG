using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using prjRGsystem.Attributes;
using prjRGsystem.Models.DbModels;
using prjRGsystem.Models.ViewModels;
using prjRGsystem.Services.DbServices;
using X.PagedList;
using static prjRGsystem.Manager.DbManager.EstimatesManager;

namespace prjRGsystem.Areas.Admin.Controllers
{
    [Area(areaName: "Admin"), AppController("估價單")]
    public class EstimateController : Controller
    {
        private ILogger<EstimateController> logger;
        private readonly EstimatesService estimatesService;
        private readonly EstimateItemsService estimateItemsService;
        public EstimateController(IHttpContextAccessor _httpContextAccessor,
               ILogger<EstimateController> _logger,
               EstimatesService _estimatesService,
               EstimateItemsService _estimateItemsService)
        {
            logger = _logger;
            estimatesService = _estimatesService;
            estimateItemsService = _estimateItemsService;
        }
        public async Task<IActionResult> Index(Criteria criteria)
        {
            int pageSize = 10;
            int pageNumber = criteria.pageNumber < 1 ? 1 : criteria.pageNumber;
            IPagedList<Estimates> items = await estimatesService.ExcuteQuery(criteria).ToPagedListAsync(pageNumber, pageSize);
            ViewData["criteria"] = criteria;
            return View(items);
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            Estimates estimate = await estimatesService.GetByIdAsync(id, false) ?? new();
            List<EstimateItems> estimateItems = await estimateItemsService.GetByEstimateId(estimate.id).ToListAsync();
            EstimateViewModel viewModel = new EstimateViewModel
            {
                Estimate = estimate,
                EstimateItems = estimateItems
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Save(int id, string data)
        {
            // 解析接收到的 JSON 数据
            var jsonData = JsonConvert.DeserializeObject<SaveData>(data);

            if (jsonData != null && jsonData.Data != null)
            {
                // 处理数据：例如保存到数据库
                foreach (var row in jsonData.Data)
                {
                    foreach (var cell in row)
                    {
                        // 这里可以将每个单元格的值保存到数据库
                        // 例如：保存 cell 到数据库的某个表格
                    }
                }
            }

            // 返回操作结果
            return Json(new { success = true, responseText = "Data saved successfully!" });
            //return Ok("");
        }
        public class SaveData
        {
            public List<List<string>> Data { get; set; }
        }
    }
}
