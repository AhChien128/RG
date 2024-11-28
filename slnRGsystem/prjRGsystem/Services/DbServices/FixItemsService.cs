using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Manager.DbManager;
using prjRGsystem.Models.DbModels;
using X.PagedList;

namespace prjRGsystem.Services.DbServices
{
    public class FixItemsService : FixItemsManager
    {
        private readonly MemberCarsManager memberCarsManager;
        private readonly MemberManager memberManager;
        private readonly ItemOrderManager itemOrderManager;
        private readonly ItemsManager itemsManager;
        public FixItemsService(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor,
            MemberCarsManager _memberCarsManager,
            MemberManager _memberManager,
             ItemOrderManager _itemOrderManager,
             ItemsManager _itemsManager) : base(_db, _httpContextAccessor)
        {
            memberCarsManager = _memberCarsManager;
            memberManager = _memberManager;
            itemOrderManager = _itemOrderManager;
            itemsManager = _itemsManager;
        }
        public async Task PrepareDataAsync(FixItems fixItem)
        {
            await PrepareDataAsync(new List<FixItems>() { fixItem });
        }
        public async Task PrepareDataAsync(List<FixItems> fixItems)
        {
            List<MemberCars> memberCars = await memberCarsManager.GetEntitiesQ().ToListAsync();
            List<Member> members = await memberManager.GetEntitiesQ().ToListAsync();
            List<ItemOrder> itemOrders = await itemOrderManager.GetEntitiesQ().ToListAsync();
            List<Items> items = await itemsManager.GetEntitiesQ().ToListAsync();
            foreach (FixItems fixItem in fixItems)
            {
                fixItem.memberCar = memberCars.Where(n => n.id == fixItem.memberCarID).FirstOrDefault() ?? new();
                fixItem.member = members.Where(n => n.id == fixItem.memberCar.memberID).FirstOrDefault() ?? new();
                fixItem.itemOrderIDs = await itemOrders.Where(n => n.fixitemsID == fixItem.id).Select(n => n.ItemID).ToListAsync() ?? new();
                fixItem.selectedItems = await itemsManager.GetByIds(fixItem.itemOrderIDs).ToListAsync();
                fixItem.total = fixItem.selectedItems.Select(n => n.productPrice).Sum();
                foreach (Items item in items)
                {
                    if (fixItem.selectedItems != null)
                    {
                        foreach (Items selectIted in fixItem.selectedItems)
                        {
                            if (item.id == selectIted.id)
                            {
                                item.isSelected = true;
                            }
                        }
                    }
                }
                fixItem.selectedItems = items;
            }

        }


    }
}
