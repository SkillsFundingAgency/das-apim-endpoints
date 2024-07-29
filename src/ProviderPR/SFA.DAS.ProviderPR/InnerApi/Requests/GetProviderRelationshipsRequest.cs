namespace SFA.DAS.ProviderPR.InnerApi.Requests;

public class GetProviderRelationshipsRequest
{
    public string? SearchTerm { get; set; }
    public bool? HasCreateCohortPermission { get; set; }
    public bool? HasRecruitmentPermission { get; set; }
    public bool? HasRecruitmentWithReviewPermission { get; set; }
    public bool? HasPendingRequest { get; set; }
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
}
