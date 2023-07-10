namespace prjRGsystem.Models.ViewModels
{
    public class ViewCurrentPage
    {
        public int TotalItemCount { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public bool IsFirstPage { get; set; }
        public bool IsLastPage { get; set; }
    }
}
