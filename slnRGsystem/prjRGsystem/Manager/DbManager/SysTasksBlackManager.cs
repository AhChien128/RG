using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Managers.DbManagers
{
    public class SysTasksBlackManager : AbstractBusinessAppEntityManager<SysTasksBlack>
    {
        public SysTasksBlackManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }

        public override IQueryable<SysTasksBlack> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.SysTasksBlack.Where(item => !item.removed);
            else
                return db.SysTasksBlack.AsNoTracking().Where(item => !item.removed);
        }

    }
}
