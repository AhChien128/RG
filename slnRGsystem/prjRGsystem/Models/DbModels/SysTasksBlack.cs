using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "權限模組-功能名稱黑名單設定資料表")]
    public class SysTasksBlack : AbstractBusinessAppEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "SysTasksBlack";

        [Display(Name = "權限模組-功能選單資料表序號")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public Int64? sysTasksId { get; set; }

        [Display(Name = "功能動作名稱")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? actionName { get; set; }

    }
}
