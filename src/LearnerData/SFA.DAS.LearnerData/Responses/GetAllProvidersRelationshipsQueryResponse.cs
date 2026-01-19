namespace SFA.DAS.LearnerData.Responses;

public class GetAllProviderRelationshipQueryResponse
{    
    public List<GetProviderRelationshipQueryResponse> GetAllProviderRelationships { get; set; }
    public int Page {  get; set; }
    public int? PageSize { get; set; }
}