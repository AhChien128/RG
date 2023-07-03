using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Managers.DbManagers
{
    public class SysUserAskForLeaveManager : AbstractBusinessAppEntityManager<SysUserAskForLeave>
    {
        public SysUserAskForLeaveManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }

        public override IQueryable<SysUserAskForLeave> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.SysUserAskForLeave.Where(item => !item.removed);
            else
                return db.SysUserAskForLeave.AsNoTracking().Where(item => !item.removed);
        }

        public IQueryable<SysUserAskForLeave> GetBySysUserIdsAndLeaveDate(List<Int64> sysUserIds, DateTime currentDate, bool isTracking = false)
        {
            return GetEnterpriseEntitiesQ().Where(item => sysUserIds.Contains(item.sysUserId) && item.startLeaveDate <= currentDate || item.endLeaveDate >= currentDate);
        }
    }
}
