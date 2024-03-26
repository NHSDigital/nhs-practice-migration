public class PaginationViewModel
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public string PageHandler { get; set; }
    
    public Guid PatientId { get; set; } 

    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;

    public int StartPage
    {
        get
        {
            int startPage = CurrentPage - 5;
            return startPage > 0 ? startPage : 1;
        }
    }

    public int EndPage
    {
        get
        {
            int endPage = CurrentPage + 5;
            int totalPages = TotalPages;
            return endPage > totalPages ? totalPages : endPage;
        }
    }
}