using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Managers.DbManagers
{
    public class SysRolesTasksBlackManager : AbstractBusinessAppEntityManager<SysRolesTasksBlack>
    {
        public SysRolesTasksBlackManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }

        public override IQueryable<SysRolesTasksBlack> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.SysRolesTasksBlack.Where(item => !item.removed);
            else
                return db.SysRolesTasksBlack.AsNoTracking().Where(item => !item.removed);
        }

        public IQueryable<SysRolesTasksBlack> GetBySysRolesTasksIds(List<Int64> sysRolesTasksIds, bool isTracking = false)
        {
            return GetEnterpriseEntitiesQ(isTracking).Where(item => sysRolesTasksIds.Contains(item.sysRolesTasksId ?? 0));
        }
        public IQueryable<SysRolesTasksBlack> GetBySysRolesTasksId(Int64 sysRolesTasksId, bool isTracking = false)
        {
            return GetBySysRolesTasksIds(new List<Int64> { sysRolesTasksId }, isTracking);
        }
    }
}
