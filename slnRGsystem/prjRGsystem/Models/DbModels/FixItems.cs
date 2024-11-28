using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "客戶維修項目")]

    public class FixItems : AbstractAppEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "FixItems";
        /// <summary>
        /// 客戶ID
        /// </summary>
        [Display(Name = "客戶ID")]
        public long memberCarID { get; set; }
        /// <summary>
        /// 維修項目ID
        /// </summary>
        [Display(Name = "維修清單ID")]
        public string? km { get; set; }
        /// <summary>
        /// 技師ID
        /// </summary>
        [Display(Name = "技師ID")]
        public long sysUserID { get; set; }
        [NotMapped]
        public MemberCars? memberCar { get; set; } 
        [NotMapped]
        public Member? member { get; set; }
        [NotMapped]
        public List<long>? itemOrderIDs { get; set; }
        [NotMapped]
        public List<Items>? selectedItems { get; set; }
        [NotMapped]
        public List<long>? selectedItemIDs { get; set; }
        [NotMapped]
        public long total { get; set; }
    }
}
