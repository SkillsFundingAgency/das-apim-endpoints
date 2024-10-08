using MediatR;

namespace SFA.DAS.EmployerPR.Application.AccountLegalEntities.Queries.GetAccountLegalEntities;

public class GetAccountLegalEntitiesQuery : IRequest<GetAccountLegalEntitiesQueryResult>
{
    public long AccountId { get; set; }
}
