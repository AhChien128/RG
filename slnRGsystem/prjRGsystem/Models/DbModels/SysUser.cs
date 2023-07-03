using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "使用者資料表")]
    public class SysUser : AbstractBusinessAppEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "SysUser";

        [Display(Name = "使用者帳號")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        [StringLength(20, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? userId { get; set; }

        [Display(Name = "使用者名稱")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        [StringLength(20, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? userName { get; set; }

        [Display(Name = "電子郵件")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? email { get; set; }

        [Display(Name = "使用者所屬部門序號")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public long userDepartmentId { get; set; }

        [Display(Name = "職稱")]
        [StringLength(20, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? title { get; set; }

        [Display(Name = "Token")]
        [StringLength(20, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? tokenId { get; set; }

        [Display(Name = "使用者密碼")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        [StringLength(100, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? password { get; set; }

        [Display(Name = "是否啟用")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public Boolean enabled { get; set; }

        [Display(Name = "是否重新設定密碼")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public Boolean resetPassword { get; set; }

        [NotMapped]
        public SysDepartment? userDepartment { get; set; }

        [NotMapped]
        public List<SysRoles>? sysRoles { get; set; }
        [NotMapped]
        public DateTime? leaveDate { get; set; }
        [NotMapped]
        public List<SysUserSubstitute>? sysUserSubstitute { get; set; }
        [NotMapped]
        public List<SysUserAskForLeave>? sysUserAskForLeave { get; set; }
        [Display(Name = "員工編號")]
        [StringLength(15, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? empId { get; set; }

        [Display(Name = "停用備註")]
        [StringLength(500, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? disableNotes { get; set; }

        [Display(Name = "備註")]
        [StringLength(500, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? remark { get; set; }
    }
}
