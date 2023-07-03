using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "客戶")]
    public class Member : AbstractAppEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "Member";

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        public string? name { get; set; }     
        /// <summary>
        /// 電話
        /// </summary>
        [Display(Name = "電話")]
        public string? phoneNumber { get; set; }
        [NotMapped]
        [Display(Name = "客戶車輛")]
        public List<MemberCars>? memberCars { get; set; }
    }
}
