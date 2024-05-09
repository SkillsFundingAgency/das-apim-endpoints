namespace SFA.DAS.EmployerPR.Application.Queries.GetAccountLegalEntities;
public class GetAccountLegalEntitiesQueryResult
{
    public IEnumerable<LegalEntity> LegalEntities { get; set; } = Enumerable.Empty<LegalEntity>();
}
