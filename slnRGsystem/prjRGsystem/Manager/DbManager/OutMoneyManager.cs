using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Managers;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Manager.DbManager
{
    public class OutMoneyManager : AbstractAppEntityManager<OutMoney>
    {
        public OutMoneyManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db)
        {


        }
        public override IQueryable<OutMoney> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.OutMoney.Where(item => !item.removed);
            else
                return db.OutMoney.AsNoTracking().Where(item => !item.removed);
        }
    }
}
