using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "權限模組-使用者身份對應表")]
    public class SysUsersRoles : AbstractBusinessAppEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "SysUsersRoles";

        [Display(Name = "權限模組-使用者帳號資料表序號")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public long sysUsersId { get; set; }

        [Display(Name = "權限模組-身份資料表序號")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public long sysRolesId { get; set; }
    }
}
