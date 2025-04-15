namespace SFA.DAS.ApprenticeshipsManage.Infrastructure;

public class PagedQuery
{
    public int Page { get; set; }
    public int? PageSize { get; set; }
}

public abstract class PagedQueryResult<T>
{
    public int Page { get; set; }
    public int TotalItems { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    public List<T> Items { get; set; } = [];
}