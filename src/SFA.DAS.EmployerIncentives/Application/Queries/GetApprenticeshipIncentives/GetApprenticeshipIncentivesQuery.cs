using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetApprenticeshipIncentives
{
    public class GetApprenticeshipIncentivesQuery : IRequest<GetApprenticeshipIncentivesResult>
    {
        public long AccountId { get ; set ; }
        public long AccountLegalEntityId { get; set; }
    }
}