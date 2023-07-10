using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using prjRGsystem.Context;
using prjRGsystem.Manager.DbManager;
using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Services.DbServices
{
    public class MemberCarsService : MemberCarsManager
    {
        private readonly MemberManager memberManager;
        public MemberCarsService(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor, MemberManager _memberManager) : base(_db, _httpContextAccessor)
        {
            memberManager = _memberManager;
        }
        public async Task PrepareDataAsync(MemberCars memberCar)
        {
            await PrepareDataAsync(new List<MemberCars>() { memberCar });
        }
        public async Task PrepareDataAsync(List<MemberCars> memberCars)
        {
            List<Member>? member = await memberManager.GetEntitiesQ().ToListAsync() ?? new();
            foreach (MemberCars memberCar in memberCars)
            {
                memberCar.member = member.Where(n => n.id == memberCar.memberID).FirstOrDefault();
            }
        }
    }
}
