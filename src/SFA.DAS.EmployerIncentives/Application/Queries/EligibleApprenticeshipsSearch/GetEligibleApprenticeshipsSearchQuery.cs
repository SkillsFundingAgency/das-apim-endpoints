using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Queries.EligibleApprenticeshipsSearch
{
    public class GetEligibleApprenticeshipsSearchQuery : IRequest<GetEligibleApprenticeshipsSearchResult>
    {
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}