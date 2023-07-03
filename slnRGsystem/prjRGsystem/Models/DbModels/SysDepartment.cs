using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "權限模組-部門資訊資料表")]
    public class SysDepartment : AbstractAppEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "SysDepartment";

        /// <summary>
        /// 上一層的部門序號
        /// </summary>
        [Display(Name = "上一層的部門序號")]
        public Int64? parentDepartmentId { get; set; }

        /// <summary>
        /// 部門名稱
        /// </summary>
        [Display(Name = "部門名稱")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? departmentName { get; set; }

        /// <summary>
        /// 部門代碼
        /// </summary>
        [Display(Name = "部門代碼")]
        [StringLength(200, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? code { get; set; }
        ///// <summary>
        ///// 業務類型
        ///// </summary>
        [NotMapped]
        public BusinessType businessTypeE { get; set; }
        [Display(Name = "業務類型:NONE:無,TECHNICAL_TEAM:技術團,ACCOUNTING:會計室,PURCHASE:秘書處採購組,TECHNOLOGY:技術合作處")]
        [StringLength(20, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? businessType
        {
            get { return businessTypeE.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    BusinessType res; if (!Enum.TryParse((string)value, out res)) throw new ApplicationException(string.Format("Can't convert '{0}' to type [{1}]", value, typeof(BusinessType)));
                    businessTypeE = res;
                }
                else
                {
                    value = BusinessType.NONE.ToString();
                    businessTypeE = BusinessType.NONE;
                }
            }
        }

        /// <summary>
        /// 企業資料表序號
        /// </summary>
        [Display(Name = "企業資料表序號")]
        [Required(ErrorMessage = "{0}是必填欄位")]
        public Int64 enterpriseId { get; set; }

        [Display(Name = "是否啟用")]
        public bool enabled { get; set; }

        ///// <summary>
        ///// 採購申請日(起)
        ///// </summary>
        //[Display(Name = "採購申請日(起)")]
        //public DateTime? purchaseRequestStartDate { get; set; }

        ///// <summary>
        ///// 採購申請日(迄)
        ///// </summary>
        //[Display(Name = "採購申請日(迄)")]
        //public DateTime? purchaseRequestEndDate { get; set; }

        [NotMapped]
        public SysDepartment? parentDepartment { get; set; }

        [NotMapped]
        public bool isSendEmail { get; set; }

        [NotMapped]
        public List<SysUser>? sysUser { get; set; }

        [Display(Name = "停用備註")]
        [StringLength(500, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? disableNotes { get; set; }

        [Display(Name = "備註")]
        [StringLength(500, ErrorMessage = "{0}長度不能超過{1}字元")]
        public string? remark { get; set; }
    }
}
