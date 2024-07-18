namespace SFA.DAS.ProviderPR.InnerApi.Responses;

public class GetProviderRelationshipsResponse
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<ProviderRelationshipModel> Employers { get; set; } = [];
}

public class ProviderRelationshipModel
{
    public long Ukprn { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public string? AgreementId { get; set; }
    public Guid? RequestId { get; set; }
    public string EmployerName { get; set; } = null!;
    public bool? HasCreateCohortPermission { get; set; }
    public bool? HasCreateAdvertPermission { get; set; }
    public bool? HasCreateAdvertWithReviewPermission { get; set; }
}
