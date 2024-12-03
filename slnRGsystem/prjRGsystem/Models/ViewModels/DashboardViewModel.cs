using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Models.ViewModels
{
    public class DashboardViewModel
    {
        public long dayTotal { get; set; }
        public List<SysUser>? sysUsers { get; set; }
        public List<FixItems>? toDayFixItems { get; set; }
        public List<OutMoney>? outMoney { get; set; }
        public long outMoneyTotal { get; set; }
    }

}
