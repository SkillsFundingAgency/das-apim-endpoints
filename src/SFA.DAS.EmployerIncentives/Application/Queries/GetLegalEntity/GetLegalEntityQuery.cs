using MediatR;
using SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntities;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntity
{
    public class GetLegalEntityQuery : IRequest<GetLegalEntityResult>
    {
        public long AccountId { get ; set ; }
        public long AccountLegalEntityId { get; set; }
    }
}