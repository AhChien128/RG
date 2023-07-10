using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Managers;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Manager.DbManager
{
    public class ItemOrderManager : AbstractAppEntityManager<ItemOrder>
    {
        public ItemOrderManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db)
        {


        }
        public override IQueryable<ItemOrder> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.ItemOrder.Where(item => !item.removed);
            else
                return db.ItemOrder.AsNoTracking().Where(item => !item.removed);
        }



    }
}
