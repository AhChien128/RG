using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "權限模組-各身份可用選單對應表")]
    public class SysRolesTasks : AbstractBusinessAppEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "SysRolesTasks";

        [Display(Name = "權限模組-身份資料表序號")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public long sysRolesId { get; set; }

        [Display(Name = "權限模組-功能選單資料表序號")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public long sysTasksId { get; set; }

        [NotMapped]
        public DataRangeType? dataRangeTypeE { get; set; }
        [Display(Name = "資料開放權限,無:NONE,只有自己:OWN,自己部門:OWN_DEPARTMENT,自己部門與底下部門:OWN_DEPARTMENT_BOTTOM_DEPARTMENT")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? dataRange
        {
            get { return dataRangeTypeE.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (!Enum.TryParse((string)value, out DataRangeType res)) throw new ApplicationException(string.Format("Can't convert '{0}' to type [{1}]", value, typeof(DataRangeType)));
                    dataRangeTypeE = res;
                }
                else
                {
                    dataRangeTypeE = null;
                }


            }
        }

        [Display(Name = "是否可以新增")]
        public Boolean? isAdd { get; set; }

        [Display(Name = "是否可以編輯")]
        public Boolean? isEdit { get; set; }

        [Display(Name = "是否可以審核")]
        public Boolean? isAudit { get; set; }

        [Display(Name = "是否可以檢視")]
        public Boolean? isView { get; set; }

        [Display(Name = "是否可以作廢")]
        public Boolean? isRemoved { get; set; }

        [Display(Name = "是否可以產報表")]
        public Boolean? isReport { get; set; }
        [Display(Name = "是否為系統管理員")]
        public Boolean? isAdmin { get; set; }

    }
}
