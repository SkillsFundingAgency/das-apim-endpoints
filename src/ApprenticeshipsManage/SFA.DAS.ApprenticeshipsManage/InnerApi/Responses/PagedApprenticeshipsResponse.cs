namespace SFA.DAS.ApprenticeshipsManage.InnerApi.Responses;

public class PagedApprenticeshipsResponse
{
    public List<Apprenticeship> Apprenticeships { get; set; } = [];

    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int Page { get; set; }
}

public class Apprenticeship
{
    public string Uln { get; set; } = "";

}
