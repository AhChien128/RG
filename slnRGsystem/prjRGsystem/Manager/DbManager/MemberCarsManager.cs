using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Managers;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Manager.DbManager
{
    public class MemberCarsManager : AbstractAppEntityManager<MemberCars>
    {
        public MemberCarsManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db)
        {


        }
        public override IQueryable<MemberCars> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.MemberCars.Where(item => !item.removed);
            else
                return db.MemberCars.AsNoTracking().Where(item => !item.removed);
        }
        public IQueryable<MemberCars> GetMemberID(long memberID)
        {
            return GetEntitiesQ().Where(n => n.memberID == memberID);
        }
    }
}
