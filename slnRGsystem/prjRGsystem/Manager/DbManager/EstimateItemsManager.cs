using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Managers;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Manager.DbManager
{
    public class EstimateItemsManager : AbstractAppEntityManager<EstimateItems>
    {
        public EstimateItemsManager(RGPropertyContext _db) : base(_db)
        {

        }
        public override IQueryable<EstimateItems> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.EstimateItems.Where(item => !item.removed);
            else
                return db.EstimateItems.AsNoTracking().Where(item => !item.removed);
        }
        public IQueryable<EstimateItems> GetByEstimateId(long estimateID)
        {
            return GetEntitiesQ().Where(n => n.estimatesId == estimateID);
        }
    }
}
