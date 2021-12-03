using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications
{
    public class GetApplicationsQuery : IRequest<GetApplicationsQueryResult>
    {
        public int PledgeId { get; set; }
        public long AccountId { get; set; }
    }
}
