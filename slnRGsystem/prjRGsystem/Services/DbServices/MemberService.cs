using Microsoft.Extensions.DependencyInjection;
using prjRGsystem.Context;
using prjRGsystem.Manager.DbManager;
using prjRGsystem.Managers.DbManagers;
using prjRGsystem.Models.DbModels;
using X.PagedList;

namespace prjRGsystem.Services.DbServices
{
    public class MemberService : MemberManager
    {
        private readonly MemberCarsService memberCarsService;
        public MemberService(RGPropertyContext _db, IHttpContextAccessor _httpContextAccessor, MemberCarsService _memberCarsService) : base(_db, _httpContextAccessor)
        {
            memberCarsService = _memberCarsService;
        }
        public async Task PrepareDataAsync(Member member)
        {
            await PrepareDataAsync(new List<Member>() { member });
        }
        public async Task PrepareDataAsync(List<Member> members)
        {
            foreach (Member member in members)
            {
                member.memberCars =await memberCarsService.GetMemberID(member.id).ToListAsync();
            }
        }
    }
}
