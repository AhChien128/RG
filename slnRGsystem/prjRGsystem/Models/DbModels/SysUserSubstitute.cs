using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "使用者代理人對應資料表")]
    public class SysUserSubstitute : AbstractBusinessAppEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "SysUserSubstitute";

        [Display(Name = "權限模組-使用者帳號資料表序號")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public long sysUserId { get; set; }

        [Display(Name = "代理人Id")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public long substituteId { get; set; }

        [NotMapped]
        public SysUser? sysUser { get; set; }

    }
}
