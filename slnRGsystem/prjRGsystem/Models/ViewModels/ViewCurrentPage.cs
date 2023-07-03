namespace prjRGsystem.Models.ViewModels
{
    public class ViewCurrentPage
    {
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public bool IsFirstPage { get; set; }
        public bool IsLastPage { get; set; }


    }
    public class ViewPageInformation
    {
        public int TotalItemCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }      
    }


}
