using MediatR;

namespace SFA.DAS.EmployerPR.Application.Queries.GetAccountLegalEntities;

public class GetAccountLegalEntitiesQuery : IRequest<GetAccountLegalEntitiesQueryResult>
{
    public long AccountId { get; set; }
}
