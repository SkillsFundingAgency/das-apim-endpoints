namespace SFA.DAS.EmployerPR.Application.EmployerRelationships.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQueryResult
{
    public GetEmployerRelationshipsQueryResult()
    {

    }

    public GetEmployerRelationshipsQueryResult(List<AccountLegalEntityPermissionsModel> accountLegalEntities)
    {
        AccountLegalEntities = accountLegalEntities;
    }

    public List<AccountLegalEntityPermissionsModel> AccountLegalEntities { get; set; } = [];
}
