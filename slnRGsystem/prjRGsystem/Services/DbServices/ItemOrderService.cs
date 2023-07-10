using prjRGsystem.Context;
using prjRGsystem.Manager.DbManager;

namespace prjRGsystem.Services.DbServices
{
    public class ItemOrderService : ItemOrderManager
    {
        public ItemOrderService(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }

    }
}
