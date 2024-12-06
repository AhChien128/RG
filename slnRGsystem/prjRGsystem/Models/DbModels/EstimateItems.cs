namespace prjRGsystem.Models.DbModels
{
    public class EstimateItems : AbstractAppEntity
    {
        public long estimatesId { get; set; }
        public string? itemName { get; set; }
        public int quantity { get; set; }
        public int unitPrice { get; set; }
        public int salary { get; set; }
        public decimal hours { get; set; }
    }
}
