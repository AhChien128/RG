using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "資料異動紀錄資料表")]
    public class SysLogDataChange : AbstractBusinessAppEntity, ILoggerEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "SysLogDataChange";

        [Display(Name = "系統請求序號")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? requestId { get; set; }

        [Display(Name = "使用者帳號")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? sysUser { get; set; }

        [Display(Name = "資料表名稱")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? tableName { get; set; }

        [Display(Name = "修改欄位")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? keyValues { get; set; }

        [Display(Name = "舊資料:新增時該欄位為空")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? oldData { get; set; }

        [Display(Name = "新資料")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? newData { get; set; }
    }
}
