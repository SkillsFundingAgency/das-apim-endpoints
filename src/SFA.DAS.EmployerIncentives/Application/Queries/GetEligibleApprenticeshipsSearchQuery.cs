using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Queries
{
    public class GetEligibleApprenticeshipsSearchQuery : IRequest<GetEligibleApprenticeshipsSearchResult>
    {
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
    }
}