using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Managers;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Manager.DbManager
{
    public class EstimatesManager : AbstractAppEntityManager<Estimates>
    {
        public EstimatesManager(RGPropertyContext _db) : base(_db)
        {

        }
        public override IQueryable<Estimates> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.Estimates.Where(item => !item.removed);
            else
                return db.Estimates.AsNoTracking().Where(item => !item.removed);
        }
        public class Criteria
        {
            /// <summary>
            /// 頁數
            /// </summary>
            public int pageNumber { get; set; }
            /// <summary>
            /// 材料名稱
            /// </summary>
            public string? keyWord { get; set; }
        }

    }
}
