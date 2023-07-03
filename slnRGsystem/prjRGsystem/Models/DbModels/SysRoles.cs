using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "權限模組-身份資料表")]
    public class SysRoles : AbstractBusinessAppEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "SysRoles";

        [Display(Name = "身份編號")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        [StringLength(20, ErrorMessage = "{0}長度不能超過{1}字元")]

        public string? roleName { get; set; }

        [NotMapped]
        public List<SysUser>? sysUsers { get; set; }

        [NotMapped]
        public string? userAccounts { get; set; }
    }
}
