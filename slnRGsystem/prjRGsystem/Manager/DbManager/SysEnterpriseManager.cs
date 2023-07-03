using prjRGsystem.Context;
using prjRGsystem.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace prjRGsystem.Managers.DbManagers
{
    public class SysEnterpriseManager : AbstractAppEntityManager<SysEnterprise>
    {
        public SysEnterpriseManager(RGPropertyContext _db) : base(_db)
        {

        }

        public override IQueryable<SysEnterprise> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.SysEnterprise.Where(item => !item.removed);
            else
                return db.SysEnterprise.AsNoTracking().Where(item => !item.removed);
        }
    }
}
