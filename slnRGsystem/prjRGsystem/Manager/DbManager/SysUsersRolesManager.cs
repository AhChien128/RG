using prjRGsystem.Context;
using prjRGsystem.Managers;
using prjRGsystem.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace prjRGsystem.Managers.DbManagers
{
    public class SysUsersRolesManager : AbstractBusinessAppEntityManager<SysUsersRoles>
    {
        public SysUsersRolesManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }

        public override IQueryable<SysUsersRoles> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.SysUsersRoles.Where(item => !item.removed);
            else
                return db.SysUsersRoles.AsNoTracking().Where(item => !item.removed);
        }

        public async Task<List<SysUsersRoles>> GetByUserIdAsync(long userId, bool isTracking = false)
        {
            return await GetEnterpriseEntitiesQ(isTracking).Where(m => m.sysUsersId == userId).ToListAsync();
        }

        public List<SysUsersRoles> GetByUserId(long userId, bool isTracking = false)
        {
            return GetEnterpriseEntitiesQ(isTracking).Where(m => m.sysUsersId == userId).ToList();
        }

        public async Task<List<SysUsersRoles>> GetByUserIdsAsync(List<long> userIds, bool isTracking = false)
        {
            return await GetEnterpriseEntitiesQ(isTracking).Where(m => userIds.Contains(m.sysUsersId)).ToListAsync();
        }

        public IQueryable<SysUsersRoles> GetBySysRoleId(long sysRoleId, bool isTracking = false)
        {
            return GetBySysRoleIds(new List<long>() { sysRoleId }, isTracking);
        }
        public IQueryable<SysUsersRoles> GetBySysRoleIds(List<long> sysRoleIds, bool isTracking = false)
        {
            return GetEnterpriseEntitiesQ(isTracking).Where(m => sysRoleIds.Contains(m.sysRolesId));
        }
        public List<SysUsersRoles> GetByUserIds(List<long> userIds, bool isTracking = false)
        {
            return GetEnterpriseEntitiesQ(isTracking).Where(m => userIds.Contains(m.sysUsersId)).ToList();
        }

    }
}
