using prjRGsystem.Context;
using prjRGsystem.Manager.DbManager;

namespace prjRGsystem.Services.DbServices
{
    public class ItemsService : ItemsManager
    {
        public ItemsService(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }

    }
}
