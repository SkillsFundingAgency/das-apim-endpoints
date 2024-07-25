using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.ProviderPR.Application.Queries.GetRelationshipByEmail;

public record GetRelationshipByEmailQueryResult
{
    public bool HasUserAccount { get; init; }
    public bool? HasOneEmployerAccount { get; init; }
    public long? AccountId { get; init; }
    public bool? HasOneLegalEntity { get; init; }
    public string? AccountLegalEntityPublicHashedId { get; init; }
    public long? AccountLegalEntityId { get; init; }
    public string? AccountLegalEntityName { get; init; }
    public bool? HasRelationship { get; init; }
    public List<Operation>? Operations { get; init; } = new List<Operation>();

    public GetRelationshipByEmailQueryResult(bool hasUserAccount, bool? hasOneEmployerAccount, long? accountId,
        bool? hasOneLegalEntity)
    {
        HasUserAccount = hasUserAccount;
        HasOneEmployerAccount = hasOneEmployerAccount;
        AccountId = accountId;
        HasOneLegalEntity = hasOneLegalEntity;
        AccountLegalEntityId = null;
    }

    public GetRelationshipByEmailQueryResult(bool hasUserAccount, bool? hasOneEmployerAccount, long? accountId,
        bool? hasOneLegalEntity, string? accountLegalEntityPublicHashedId, long? accountLegalEntityId, string? accountLegalEntityName, bool? hasRelationship, List<Operation> operations)
    {
        HasUserAccount = hasUserAccount;
        HasOneEmployerAccount = hasOneEmployerAccount;
        AccountId = accountId;
        HasOneLegalEntity = hasOneLegalEntity;
        AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
        AccountLegalEntityId = accountLegalEntityId;
        AccountLegalEntityName = accountLegalEntityName;
        HasRelationship = hasRelationship;
        Operations = operations;
    }
}