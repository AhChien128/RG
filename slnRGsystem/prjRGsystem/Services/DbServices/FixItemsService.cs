using prjRGsystem.Context;
using prjRGsystem.Manager.DbManager;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Services.DbServices
{
    public class FixItemsService : FixItemsManager
    {
        public FixItemsService(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor) : base(_db, _httpContextAccessor)
        {

        }
        public async Task PrepareDataAsync(FixItems fixItem)
        {
            await PrepareDataAsync(new List<FixItems>() { fixItem });
        }
        public async Task PrepareDataAsync(List<FixItems> fixItems)
        {

            foreach (FixItems fixItem in fixItems)
            {

            }
        }


    }
}
