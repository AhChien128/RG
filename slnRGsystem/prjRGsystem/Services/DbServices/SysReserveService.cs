using prjRGsystem.Context;
using prjRGsystem.Manager.DbManager;

namespace prjRGsystem.Services.DbServices
{
    public class SysReserveService : SysReserveManager
    {
        public SysReserveService(RGPropertyContext _db) : base(_db)
        {

        }
    }
}
