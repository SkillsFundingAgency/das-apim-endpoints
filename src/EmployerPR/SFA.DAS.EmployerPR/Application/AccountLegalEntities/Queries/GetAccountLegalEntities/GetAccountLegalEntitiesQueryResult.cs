namespace SFA.DAS.EmployerPR.Application.AccountLegalEntities.Queries.GetAccountLegalEntities;
public class GetAccountLegalEntitiesQueryResult
{
    public IEnumerable<LegalEntity> LegalEntities { get; set; } = Enumerable.Empty<LegalEntity>();
}
