using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Managers;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Manager.DbManager
{
    public class SysReserveManager : AbstractAppEntityManager<SysReserve>
    {
        public SysReserveManager(RGPropertyContext _db) : base(_db)
        {
        }
        public override IQueryable<SysReserve> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.SysReserve.Where(item => !item.removed);
            else
                return db.SysReserve.AsNoTracking().Where(item => !item.removed);
        }
        public IQueryable<SysReserve> GetStrEndDate(DateTime strDate, DateTime endDate)
        {
            return GetEntitiesQ().Where(item => item.stratDate >= strDate && item.endDate <= endDate);
        }
    }
}
