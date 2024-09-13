using MediatR;

namespace SFA.DAS.EmployerPR.Application.EmployerAccountsLegalEntities.Queries.GetAccountLegalEntities;

public class GetAccountLegalEntitiesQuery : IRequest<GetAccountLegalEntitiesQueryResult>
{
    public long AccountId { get; set; }
}
