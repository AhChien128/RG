using prjRGsystem.Context;
using prjRGsystem.Managers.DbManagers;

namespace prjRGsystem.Services.DbServices
{
    public class SysEnterpriseService : SysEnterpriseManager
    {
        public SysEnterpriseService(RGPropertyContext _db) : base(_db)
        {

        }
    }
}
