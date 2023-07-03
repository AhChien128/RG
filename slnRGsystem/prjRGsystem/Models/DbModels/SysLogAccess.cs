using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "系統存取紀錄資料表")]
    public class SysLogAccess : AbstractBusinessAppEntity, ILoggerEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "SysLogAccess";
        /// <summary>
        /// 來源IP位址
        /// </summary>
        [Display(Name = "來源IP位址")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? remoteIp { get; set; }
        /// <summary>
        /// 系統請求序號
        /// </summary>
        [Display(Name = "系統請求序號")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? requestId { get; set; }
        /// <summary>
        /// 使用者帳號
        /// </summary>
        [Display(Name = "使用者帳號")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? sysUser { get; set; }
        /// <summary>
        /// 控制器名稱
        /// </summary>
        [Display(Name = "控制器名稱")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? controllerName { get; set; }
        /// <summary>
        /// 方法名稱
        /// </summary>
        [Display(Name = "方法名稱")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? actionName { get; set; }
        /// <summary>
        /// 區域名稱
        /// </summary>
        [Display(Name = "區域名稱")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? areaName { get; set; }
        /// <summary>
        /// 查詢內容
        /// </summary>
        [Display(Name = "查詢內容")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? queryString { get; set; }
        /// <summary>
        /// 控制器顯示名稱
        /// </summary>
        [Display(Name = "控制器顯示名稱")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? controllerDisplayName { get; set; }
        /// <summary>
        /// 動作顯示名稱
        /// </summary>
        [Display(Name = "動作顯示名稱")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? actionDisplayName { get; set; }
        /// <summary>
        /// 瀏覽器類型
        /// </summary>
        [Display(Name = "瀏覽器類型")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]        
        public string? browser { get; set; }
        /// <summary>
        /// 瀏覽器版本
        /// </summary>
        [Display(Name = "瀏覽器版本")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? browserVersion { get; set; }
        /// <summary>
        /// 作業系統平台
        /// </summary>
        [Display(Name = "作業系統平台")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? os { get; set; }
        
        
        

    }
}
