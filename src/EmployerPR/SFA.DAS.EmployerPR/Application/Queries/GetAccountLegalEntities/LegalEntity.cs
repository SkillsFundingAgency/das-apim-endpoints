using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;

namespace SFA.DAS.EmployerPR.Application.Queries.GetAccountLegalEntities;

public class LegalEntity
{
    public string? AccountLegalEntityName { get; set; }
    public string? AccountLegalEntityPublicHashedId { get; set; }
    public long? AccountLegalEntityId { get; set; }

    public static implicit operator LegalEntity(GetAccountLegalEntityResponse source)
    {
        return new LegalEntity
        {
            AccountLegalEntityName = source.Name,
            AccountLegalEntityPublicHashedId = source.AccountLegalEntityPublicHashedId,
            AccountLegalEntityId = source.AccountLegalEntityId
        };
    }
}