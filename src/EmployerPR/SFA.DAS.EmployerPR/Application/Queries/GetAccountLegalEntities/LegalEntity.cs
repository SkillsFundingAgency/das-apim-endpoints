namespace SFA.DAS.EmployerPR.Application.Queries.GetAccountLegalEntities;

public class LegalEntity
{
    public string? Name { get; set; }
    public string? PublicHashedId { get; set; }

    public static implicit operator LegalEntity(AccountLegalEntity source)
    {
        return new LegalEntity
        {
            Name = source.Name,
            PublicHashedId = source.AccountLegalEntityPublicHashedId
        };
    }
}