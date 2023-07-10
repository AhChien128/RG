using System.ComponentModel.DataAnnotations;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "維修單")]
    public class ItemOrder : AbstractAppEntity
    {
        public long fixitemsID { get; set; }
        public long ItemID { get; set; }
    }
}
