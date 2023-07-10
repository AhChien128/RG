using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Models.ViewModels
{
    public class FixItemViewModel
    {
        public List<MemberCars>? memberCars { get; set; }
        public List<Items>? items { get; set; }
        public List<FixItems>? fixItems { get; set; }
    }
}
