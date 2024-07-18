namespace SFA.DAS.ProviderPR.InnerApi.Requests;

public class GetProviderRelationshipsRequest
{
    public string? EmployerName { get; set; }
    public bool HasPendingRequest { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public bool? HasCreateCohortPermission { get; set; }
    public bool? HasRecruitWithReviewPermission { get; set; }
    public bool? HasRecruitPermission { get; set; }
    public bool? HasNoRecruitPermissions { get; set; }
}
