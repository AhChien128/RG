using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "權限模組-企業資訊資料表")]
    public class SysEnterprise : AbstractAppEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "SysEnterprise";

        /// <summary>
        /// 網域路徑
        /// </summary>
        [Display(Name = "網域路徑")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? domainName { get; set; }

        /// <summary>
        /// 是否除錯
        /// </summary>
        [Display(Name = "是否除錯")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public bool isDeBug { get; set; }

        /// <summary>
        /// 企業名稱
        /// </summary>
        [Display(Name = "企業名稱")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? companyName { get; set; }

        /// <summary>
        /// 電子郵件-1
        /// </summary>
        [Display(Name = "電子郵件-1")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? email1 { get; set; }
    }
}
