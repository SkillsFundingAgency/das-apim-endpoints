namespace SFA.DAS.ProviderPR.InnerApi.Responses;

public class GetProviderRelationshipsResponse
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalCount { get; set; }
    public bool HasAnyRelationships { get; set; }
    public IEnumerable<ProviderRelationshipModel> Employers { get; set; } = [];
}

public class ProviderRelationshipModel
{
    public long? AccountLegalEntityId { get; set; }
    public string? AgreementId { get; set; }
    public Guid? RequestId { get; set; }
    public string EmployerName { get; set; } = null!;
    public bool HasCreateCohortPermission { get; set; }
    public bool HasRecruitmentPermission { get; set; }
    public bool HasRecruitmentWithReviewPermission { get; set; }
}
