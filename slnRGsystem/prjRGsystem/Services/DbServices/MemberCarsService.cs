using prjRGsystem.Context;
using prjRGsystem.Manager.DbManager;

namespace prjRGsystem.Services.DbServices
{
    public class MemberCarsService : MemberCarsManager
    {
        public MemberCarsService(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }

    }
}
