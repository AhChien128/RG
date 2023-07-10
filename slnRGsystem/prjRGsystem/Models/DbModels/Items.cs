using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjRGsystem.Models.DbModels
{
    [Display(Name = "材料")]
    public class Items : AbstractAppEntity
    {
        [NotMapped]
        public const string TABLE_NAME = "Items";
        /// <summary>
        /// 材料名稱
        /// </summary>
        [Display(Name = "名稱")]
        public string? productName { get; set; }
        /// <summary>
        /// 材料價錢
        /// </summary>
        [Display(Name = "價錢")]
        public string? productPrice { get; set; }
        /// <summary>
        /// 材料成本
        /// </summary>
        [Display(Name = "成本")]
        public string? productCost { get; set; }
        /// <summary>
        /// 材料描述
        /// </summary>
        [Display(Name = "描述")]
        public string? productDescribe { get; set; }
        /// <summary>
        /// 材料品牌
        /// </summary>
        [Display(Name = "品牌or廠商")]
        public string? productBrand { get; set; }

    }
}
