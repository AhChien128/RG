using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "API使用紀錄資料表")]
    public class SysLogApi : AbstractBusinessAppEntity, ILoggerEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "SysLogApi";

        [Display(Name = "呼叫URL")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? url { get; set; }

        [Display(Name = "呼叫port")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? port { get; set; }

        [Display(Name = "查詢參數")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? queryData { get; set; }

        [Display(Name = "呼叫方式:GET,POST")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? httpMethodType { get; set; }

        [Display(Name = "回傳參數")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? response { get; set; }
    }
}
