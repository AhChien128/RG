using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Managers.DbManagers
{
    public class SysRolesTasksManager : AbstractBusinessAppEntityManager<SysRolesTasks>
    {
        public SysRolesTasksManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }

        public override IQueryable<SysRolesTasks> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.SysRolesTasks.Where(item => !item.removed);
            else
                return db.SysRolesTasks.AsNoTracking().Where(item => !item.removed);
        }

        public IQueryable<SysRolesTasks> GetBySysRolesIds(List<long> rolesIds, bool isTracking = false)
        {
            return GetEnterpriseEntitiesQ(isTracking).Where(m => rolesIds.Contains(m.sysRolesId));
        }
        public IQueryable<SysRolesTasks> GetBySysRolesId(long rolesId, bool isTracking = false)
        {
            return GetBySysRolesIds(new List<long> { rolesId }, isTracking);
        }

        public Task<List<SysRolesTasks>> GetByRolesIdsAsync(List<long> rolesIds, bool isTracking = false)
        {
            return GetEnterpriseEntitiesQ(isTracking).Where(m => rolesIds.Contains(m.sysRolesId)).ToListAsync();
        }

        public async Task<List<SysRolesTasks>> GetByRolesIdsAsync(long sysRolesId, bool isTracking = false)
        {
            return await GetEntitiesQ(isTracking).Where(m => m.sysRolesId == sysRolesId).ToListAsync();
        }

    }
}
