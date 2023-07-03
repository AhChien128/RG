
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "權限模組-功能選單權限及黑名單關聯資料表")]
    public class SysRolesTasksBlack : AbstractBusinessAppEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "SysRolesTasksBlack";

        [Display(Name = "權限模組-功能名稱黑名單設定資料表序號")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public Int64? sysTasksBlackId { get; set; }

        [Display(Name = "權限模組-各身份可用選單對應表序號")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public Int64? sysRolesTasksId { get; set; }


    }
}
