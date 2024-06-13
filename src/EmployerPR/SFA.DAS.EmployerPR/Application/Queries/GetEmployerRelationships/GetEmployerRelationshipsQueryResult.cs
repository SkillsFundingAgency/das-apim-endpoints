namespace SFA.DAS.EmployerPR.Application.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQueryResult
{
    public GetEmployerRelationshipsQueryResult()
    {

    }

    public GetEmployerRelationshipsQueryResult(List<AccountLegalEntityPermissionsModel> accountLegalEntities)
    {
        this.AccountLegalEntities = accountLegalEntities;
    }

    public List<AccountLegalEntityPermissionsModel> AccountLegalEntities { get; set; } = [];
}
