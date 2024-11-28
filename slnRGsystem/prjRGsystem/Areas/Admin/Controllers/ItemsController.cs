using Aspose.Cells;
using Aspose.Cells.Drawing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using prjRGsystem.Attributes;
using prjRGsystem.Manager.DbManager;
using prjRGsystem.Managers.DbManagers;
using prjRGsystem.Models;
using prjRGsystem.Models.DbModels;
using prjRGsystem.Services.DbServices;
using System.Drawing.Printing;
using System.Reflection.Metadata;
using X.PagedList;
using static prjRGsystem.Manager.DbManager.ItemsManager;
using static System.Collections.Specialized.BitVector32;

namespace prjRGsystem.Areas.Admin.Controllers
{
    [Area(areaName: "Admin"), AppController("材料維護")]
    public class ItemsController : Controller
    {
        private readonly ItemsService itemsService;
        private ILogger<ItemsController> logger;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ItemsController(IHttpContextAccessor _httpContextAccessor, ILogger<ItemsController> _logger, ItemsService _itemsService, IWebHostEnvironment _webHostEnvironment)
        {
            itemsService = _itemsService;
            logger = _logger;
            webHostEnvironment = _webHostEnvironment;
        }
        public async Task<IActionResult> Index(Criteria criteria)
        {
            int pageSize = 10;
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

        [HttpPost, AppAction("材料匯入")]
        public async Task<string> Import(IFormFile file)
        {
            Dictionary<String, Object> valueObject = new Dictionary<String, Object>();
            try
            {
                List<string> itemColumnList = new List<string>()
                {
                    "廠商","名稱","價錢","成本","描述"
                };
                Workbook workbook = new Workbook(file.OpenReadStream());
                Worksheet worksheet = workbook.Worksheets[0];
                int maxDataRow = worksheet.Cells.MaxDataRow;
                int maxDataColumn = worksheet.Cells.MaxDataColumn;
                int rowBeginCount = 4;
                string errorMessage = "";
                List<Items> l_items = new List<Items>();
                if (maxDataRow >= 4)
                {
                    for (int i = rowBeginCount; i <= maxDataRow; i++)
                    {
                        Items item = new Items();
                        for (int j = 0; j <= maxDataColumn; j++)
                        {
                            var value = worksheet.Cells[i, j].Value;
                            errorMessage += CheckAndPrepareItemData(itemColumnList, i, j, item, value);
                        }
                        l_items.Add(item);
                    }
                }
                else
                    errorMessage += "阿~材料資訊呢";

                if (errorMessage.Length > 0)
                {
                    valueObject.Add("success", false);
                    valueObject.Add("message", errorMessage);
                }
                else
                {
                    if (l_items.Count > 0)
                    {
                        foreach (Items Item in l_items)
                        {
                            Items? insetItem = new Items();
                            if (insetItem != null)
                            {
                                insetItem.productBrand = Item.productBrand;
                                insetItem.productPrice= Item.productPrice;
                                insetItem.productName = Item.productName;
                                insetItem.productDescribe = Item.productDescribe;
                                insetItem.productCost = Item.productCost;
                                await itemsService.SaveAsync(insetItem);
                            }
                        }
                    }
                    valueObject.Add("success", true);
                    valueObject.Add("message", "匯入成功");
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

        [HttpPost, AppAction("材料匯入範例下載")]
        public IActionResult ImportExample()
        {
            string l_strReportPath = Path.GetFullPath("Doc\\Template\\");
            string downloadFileName = "材料匯入格式.xlsx";
            Workbook workbook = new Workbook(l_strReportPath + downloadFileName);

            MemoryStream licensestream = SysFileManager.GetAsposeLicense(webHostEnvironment);
            MemoryStream excelStream = new MemoryStream();
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense(licensestream);

            workbook.Save(excelStream, Aspose.Cells.SaveFormat.Xlsx);
            excelStream.Position = 0;
            var res = new FileStreamResult(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            res.FileDownloadName = "材料匯入格式.xlsx";
            return res;
        }

        public async Task<IActionResult> IndexPrintOut(ItemsManager.Criteria criteria)
        {
            #region 浮水印與建立excel文件
            MemoryStream licensestream = SysFileManager.GetAsposeLicense(webHostEnvironment);
            MemoryStream excelStream = new MemoryStream();
            Aspose.Cells.License license = new Aspose.Cells.License();
            license.SetLicense(licensestream);

            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];
            #endregion

            #region Data
            List<Items> itemss = await itemsService.ExcuteQuery(criteria).ToListAsync();
          
            #endregion

            #region Title
            string[] l_Title = { "廠商", "名稱", "價錢", "成本", "描述" };
            for (int i = 0; i < l_Title.Length; i++)
            {
                worksheet.Cells[0, i].Value = l_Title[i];
                worksheet.Cells[0, i].SetStyle(StyleHender());
            }
            #endregion

            #region Width
            //worksheet.Cells.SetColumnWidth(0, 50);
            //worksheet.Cells.SetColumnWidth(1, 20);
            //worksheet.Cells.SetColumnWidth(2, 10);
            //worksheet.Cells.SetColumnWidth(3, 18);
            //worksheet.Cells.SetColumnWidth(4, 25);
            //worksheet.Cells.SetColumnWidth(5, 25);
            //worksheet.Cells.SetColumnWidth(6, 25);
            //worksheet.Cells.SetColumnWidth(7, 25);
            //worksheet.Cells.SetColumnWidth(8, 15);
            #endregion

            #region Content
            int rowIndex = 1;
            foreach (Items item in itemss)
            {
                worksheet.Cells[rowIndex, 0].Value = item.productBrand;
                worksheet.Cells[rowIndex, 1].Value = item.productName;
                worksheet.Cells[rowIndex, 2].Value = item.productPrice;
                worksheet.Cells[rowIndex, 3].Value = item.productCost;
                worksheet.Cells[rowIndex, 4].Value = item.productDescribe;
                Aspose.Cells.Range rangeCount = worksheet.Cells.CreateRange(rowIndex, 0, 1, 5);
                rangeCount.SetStyle(StyleContent());
                rowIndex++;
            }
            #endregion

            // Create an object for AutoFitterOptions
            AutoFitterOptions options = new AutoFitterOptions();

            // Set auto-fit for merged cells
            options.AutoFitMergedCells = true;

            // Autofit rows in the sheet(including the merged cells)
            worksheet.AutoFitRows(options);

            workbook.Save(excelStream, Aspose.Cells.SaveFormat.Xlsx);
            excelStream.Position = 0;
            var res2 = new FileStreamResult(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            res2.FileDownloadName = "材料資訊表.xlsx";
            return res2;
        }

        /// <summary>
        /// 匯入的材料防呆
        /// </summary>
        /// <param name="columnList">橫向表頭</param>
        /// <param name="rowCount">第幾筆資料(列)</param>
        /// <param name="columnCount">第幾個欄位(欄)</param>
        /// <param name="importPurchaseOrder">空資料,處理後存入list</param>
        /// <param name="cellValue">該列/欄,傳入的數據</param>
        /// <param name="manufacturers">所有得標廠商</param>
        /// <param name="dataNumbers">案件編號表</param>
        /// <returns></returns>
        public string CheckAndPrepareItemData(List<string> columnList, int rowCount, int columnCount, Items items, object cellValue)
        {
            // "廠商","名稱","價錢","價","錢成本","描述"
            string alertMessage = "";
            if (columnCount == 0)
            {
                string? productBrand = cellValue == null ? "" : cellValue.ToString();
                items.productBrand = productBrand;
            }
            if (columnCount == 1)
            {
                string? itemName = cellValue == null ? "" : cellValue.ToString();
                if (!string.IsNullOrEmpty(itemName))
                    items.productName = itemName;
                else
                    alertMessage += "「" + columnList[columnCount] + "」" + "第" + (rowCount + 1) + "列，請輸入名稱\n";
            }
            else if (columnCount == 2)
            {
                int dataUnitPrice = 0;

                if (cellValue != null)
                {
                    if (int.TryParse(cellValue.ToString(), out dataUnitPrice))
                    {
                        items.productPrice = dataUnitPrice;
                    }
                    else
                        alertMessage += "「" + columnList[columnCount] + "」" + "第" + (rowCount + 1) + "列，格式錯誤\n";
                }
                else
                    alertMessage += "「" + columnList[columnCount] + "」" + "第" + (rowCount + 1) + "列，請輸入價錢\n";
            }
            else if (columnCount == 3)
            {
                int dataUnitCost = 0;

                if (cellValue != null)
                {
                    if (int.TryParse(cellValue.ToString(), out dataUnitCost))
                    {
                        items.productCost = dataUnitCost;
                    }
                    else
                        alertMessage += "「" + columnList[columnCount] + "」" + "第" + (rowCount + 1) + "列，格式錯誤\n";
                }
                else
                    alertMessage += "「" + columnList[columnCount] + "」" + "第" + (rowCount + 1) + "列，請輸入成本\n";
            }
            else if (columnCount == 4)
            {
                string? productDescribe = cellValue == null ? "" : cellValue.ToString();
                items.productDescribe = productDescribe;
            }
            return alertMessage;
        }
        private Aspose.Cells.Style StyleContent()
        {
            Workbook workbook = new Workbook();
            Aspose.Cells.Style styleHender = workbook.CreateStyle();
            //標題-style
            styleHender.Font.Name = "標楷體";//字型            
            styleHender.Font.Size = 12;//字體大小
            styleHender.Font.IsBold = false;//加粗
            styleHender.IsTextWrapped = true;//自動換行
            styleHender.HorizontalAlignment = TextAlignmentType.Center;//水平對齊
            styleHender.HorizontalAlignment = TextAlignmentType.Bottom;
            styleHender.VerticalAlignment = TextAlignmentType.Center;//垂直對齊
            styleHender.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = CellBorderType.Thin;//上方框線
            styleHender.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = CellBorderType.Thin;//左邊框線
            styleHender.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = CellBorderType.Thin;//下方框線
            styleHender.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = CellBorderType.Thin;//右邊框線
            return styleHender;
        }

        private Aspose.Cells.Style StyleHender()
        {
            Workbook workbook = new Workbook();
            Aspose.Cells.Style styleHender = workbook.CreateStyle();
            //標題-style
            styleHender.Font.Name = "標楷體";//字型            
            styleHender.Font.Size = 14;//字體大小
            styleHender.Font.IsBold = false;//加粗
            styleHender.IsTextWrapped = true;//自動換行
            styleHender.HorizontalAlignment = TextAlignmentType.Center;//水平對齊
            styleHender.HorizontalAlignment = TextAlignmentType.Bottom;
            styleHender.VerticalAlignment = TextAlignmentType.Center;//垂直對齊
            styleHender.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = CellBorderType.Thin;//上方框線
            styleHender.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = CellBorderType.Thin;//左邊框線
            styleHender.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = CellBorderType.Thin;//下方框線
            styleHender.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = CellBorderType.Thin;//右邊框線
            return styleHender;
        }


    }
}
