using Aspose.Cells;
using System.ComponentModel.DataAnnotations;

namespace prjRGsystem.Models.DbModels
{
    public class OutMoney : AbstractAppEntity
    {
        [Display(Name = "項目")]
        public string? item { get; set; }
        [Display(Name = "金額")]
        public int itemMoney { get; set; }
    }
}
