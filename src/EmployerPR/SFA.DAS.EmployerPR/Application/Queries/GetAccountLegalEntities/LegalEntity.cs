using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;

namespace SFA.DAS.EmployerPR.Application.Queries.GetAccountLegalEntities;

public class LegalEntity
{
    public string? Name { get; set; }
    public string? PublicHashedId { get; set; }
    public long? LegalEntityId { get; set; }

    public static implicit operator LegalEntity(GetAccountLegalEntityResponse source)
    {
        return new LegalEntity
        {
            Name = source.Name,
            PublicHashedId = source.AccountLegalEntityPublicHashedId,
            LegalEntityId = source.AccountLegalEntityId
        };
    }
}