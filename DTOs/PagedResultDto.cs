namespace JobApplicationTracker.DTOs;

// Generic wrapper — je kono list-er sathe pagination info (total count,
// total pages) pathanor jonno. Frontend eta diye "Page 1 of 5" dekhabe.
public class PagedResultDto<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages =>
        PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
}