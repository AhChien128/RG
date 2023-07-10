using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using prjRGsystem.Attributes;
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
        public FixItemsController(IHttpContextAccessor _httpContextAccessor, ILogger<FixItemsController> _logger
            , MemberCarsService __memberCarsService, MemberService _memberService, ItemsService _itemsService, FixItemsService _fixItemsService)
        {
            if (_httpContextAccessor.HttpContext is not null)
                session = _httpContextAccessor.HttpContext.Session;
            logger = _logger;
            memberCarsService = __memberCarsService;
            memberService = _memberService;
            itemsService = _itemsService;
            fixItemsService = _fixItemsService;
        }
        public async Task<IActionResult> Edit(string carNumber)
        {

            FixItemViewModel fixItemViewModel = new();
            if (!string.IsNullOrEmpty(carNumber))
            {
                fixItemViewModel.memberCars=await memberCarsService.GetEntitiesQ().Where(n => n.carNumber== carNumber).ToListAsync();
                MemberCars memberCars = await memberCarsService.GetEntitiesQ().Where(n => n.carNumber == carNumber).FirstAsync();
                fixItemViewModel.items = await itemsService.GetEntitiesQ().ToListAsync();
                fixItemViewModel.fixItems = await fixItemsService.GetEntitiesQ().Where(n=>n.memberCarID== memberCars.memberID).ToListAsync();
            }
            else
            {
                fixItemViewModel.memberCars = await memberCarsService.GetEntitiesQ().OrderBy(n => n.carNumber).ToListAsync();
                fixItemViewModel.items = await itemsService.GetEntitiesQ().ToListAsync();
            }


            return View(fixItemViewModel);
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
