using prjRGsystem.Context;
using prjRGsystem.Manager.DbManager;
using prjRGsystem.Managers.DbManagers;

namespace prjRGsystem.Services.DbServices
{
    public class OutMoneyService: OutMoneyManager
    {
        public OutMoneyService(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        { 
        
        }
    }


}
