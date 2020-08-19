using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntities
{
    public class GetLegalEntitiesQuery : IRequest<GetLegalEntitiesResult>
    {
        public long AccountId { get ; set ; }
    }
}