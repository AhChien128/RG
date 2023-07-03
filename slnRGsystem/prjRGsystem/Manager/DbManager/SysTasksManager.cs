using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Managers;
using prjRGsystem.Models.DbModels;


namespace prjRGsystem.Managers.DbManagers
{
    public class SysTasksManager : AbstractBusinessAppEntityManager<SysTasks>
    {
        public SysTasksManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }

        public override IQueryable<SysTasks> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.SysTasks.Where(item => !item.removed);
            else
                return db.SysTasks.AsNoTracking().Where(item => !item.removed);
        }

    }
}
