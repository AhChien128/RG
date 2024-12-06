using prjRGsystem.Models.DbModels;

namespace prjRGsystem.Models.ViewModels
{
    public class EstimateViewModel
    {
        public Estimates? Estimate { get; set; }
        public List<EstimateItems>? EstimateItems { get; set; }
    }
}
