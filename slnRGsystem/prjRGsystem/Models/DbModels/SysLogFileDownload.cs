using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "檔案下載記錄資料表")]
    public class SysLogFileDownload : AbstractBusinessAppEntity, ILoggerEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "SysLogFileDownload";

        [Display(Name = "來源IP位址")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? remoteIp { get; set; }

        public string? accessUserId { get; set; }

        [Display(Name = "資料表名稱")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? tableName { get; set; }

        [Display(Name = "檔案名稱")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? fileName { get; set; }

        [Display(Name = "顯示檔案名稱")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? displayName { get; set; }
    }
}
