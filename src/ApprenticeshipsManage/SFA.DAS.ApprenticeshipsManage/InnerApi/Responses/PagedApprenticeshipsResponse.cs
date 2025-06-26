namespace SFA.DAS.ApprenticeshipsManage.InnerApi.Responses;

public class PagedApprenticeshipsResponse
{
    public List<Learning> Items { get; set; } = [];

    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int Page { get; set; }
}

public class Learning
{
    public string Uln { get; set; } = "";

}
