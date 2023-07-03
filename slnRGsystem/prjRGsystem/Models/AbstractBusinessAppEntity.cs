using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models
{
    public class AbstractBusinessAppEntity : AbstractAppEntity
    {
        public Int64 enterpriseId { get; set; }
        public Int64 departmentId { get; set; }

        [NotMapped]
        public bool nonSaveAutowiredEnterpriseId = false;//設定TRUE時儲存時不自動壓企業序號
        [NotMapped]
        public bool nonSaveAutowiredDepartmentId = false;//設定TRUE時儲存時不自動壓部門序號
    }
}
