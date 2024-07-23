using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.ProviderPR.Application.Queries.GetEasUserByEmail;
public class GetEasUserByEmailQueryResult
{
    public bool HasUserAccount { get; set; }
    public bool HasOneEmployerAccount { get; set; }
    public long? AccountId { get; set; }
    public bool HasOneLegalEntity { get; set; }

    public string? AccountLegalEntityPublicHashedId { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public string? AccountLegalEntityName { get; set; }
    public bool HasRelationship { get; set; }
    public List<Operation>? Operations { get; set; }
}
