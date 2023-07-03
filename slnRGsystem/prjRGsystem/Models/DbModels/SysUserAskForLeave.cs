using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "使用者請假對應資料表")]
    public class SysUserAskForLeave : AbstractBusinessAppEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "SysUserAskForLeave";
        [Display(Name = "權限模組-使用者帳號資料表序號")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public long sysUserId { get; set; }
        [Display(Name = "休假日期")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public DateTime startLeaveDate { get; set; }
        [Display(Name = "休假日期")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public DateTime endLeaveDate { get; set; }
    }
}
