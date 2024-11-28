using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Models.ViewModels
{
    public class FixItemViewModel
    {
        public List<MemberCars>? memberCars { get; set; }
        public List<Items>? selecteditems { get; set; }
        public List<Items>? items { get; set; }

        public long fixitemsID { get; set; }
        public long memberCarsID { get; set; }
        public string? memberCarsKm { get; set; }
        //public long memberID { get; set; }
    }
}
