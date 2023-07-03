using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "權限模組-功能選單資料表")]
    public class SysTasks : AbstractBusinessAppEntity, ITreeEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "SysTasks";
        [NotMapped]
        public long parentId { get => this.parentTasksId; set => parentTasksId = value; }

        [Display(Name = "上一層功能選單序號")]
        public long parentTasksId { get; set; }

        [Display(Name = "功能編號")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? taskActionId { get; set; }

        [Display(Name = "區域名稱")]
        [StringLength(50, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? areaName { get; set; }

        [Display(Name = "功能名稱")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        [StringLength(20, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? taskName { get; set; }

        [Display(Name = "控制器名稱")]
        [StringLength(50, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? controllerName { get; set; }

        [Display(Name = "方法名稱")]
        [StringLength(50, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? actionName { get; set; }

        [Display(Name = "選單圖示名稱")]
        [StringLength(20, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? iconName { get; set; }

        [Display(Name = "排序")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public int taskSort { get; set; }

        [NotMapped]
        public LinkedList<SysTasks>? childSysTask { get; set; }
    }
}
