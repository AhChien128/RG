using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "客戶車輛")]
    public class MemberCars : AbstractAppEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "MemberCars";
        /// <summary>
        /// 客戶關聯ID
        /// </summary>
        [Display(Name = "客戶關聯ID")]
        public Int64 memberID { get; set; }
        /// <summary>
        /// 車牌
        /// </summary>
        [Display(Name = "車牌")]
        public string? carNumber { get; set; }
        /// <summary>
        /// 車款
        /// </summary>
        [Display(Name = "車款")]
        public string? carCategroy { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        [Display(Name = "品牌")]
        public string? brand { get; set; }

    }
}
