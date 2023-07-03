using Microsoft.EntityFrameworkCore;
using prjRGsystem.Context;
using prjRGsystem.Managers;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Manager.DbManager
{
    public class MemberManager : AbstractAppEntityManager<Member>
    {
        public MemberManager(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db)
        {


        }
        public override IQueryable<Member> GetEntitiesQ(bool isTracking = false)
        {
            if (isTracking)
                return db.Member.Where(item => !item.removed);
            else
                return db.Member.AsNoTracking().Where(item => !item.removed);
        }

    }
}
