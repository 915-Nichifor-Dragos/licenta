namespace HotelManagement.Models.ViewModels;

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }
    public string CurrentSort { get; private set; }
    public bool CurrentOrder { get; private set; }

    public PaginatedList(ICollection<T> items, int count, int pageIndex, int pageSize, string currentSort, bool currentOrder)
    {
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageIndex = (pageIndex > TotalPages) ? TotalPages : pageIndex;
        CurrentSort = currentSort;
        CurrentOrder = currentOrder;

        this.AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;
}