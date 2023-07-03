using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Managers.DbManagers
{
    public class SysUserSubstituteManager : AbstractBusinessAppEntityManager<SysUserSubstitute>
    {
        public SysUserSubstituteManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }

        public override IQueryable<SysUserSubstitute> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.SysUserSubstitute.Where(item => !item.removed);
            else
                return db.SysUserSubstitute.AsNoTracking().Where(item => !item.removed);
        }

        public IQueryable<SysUserSubstitute> GetBySubstituteIds(List<long> substituteIds, bool isTracking = false)
        {
            return GetEnterpriseEntitiesQ(isTracking).Where(item => substituteIds.Contains(item.substituteId));
        }

        public IQueryable<SysUserSubstitute> GetBySubstituteId(long substituteId, bool isTracking = false)
        {
            return GetBySubstituteIds(new() { substituteId }, isTracking);
        }
    }
}
