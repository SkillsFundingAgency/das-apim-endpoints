using MediatR;
using SFA.DAS.LevyTransferMatching.Infrastructure;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications
{
    public class GetApplicationsQuery : PagedQuery, IRequest<GetApplicationsQueryResult>
    {
        public int PledgeId { get; set; }
        public string SortOrder { get; set; }
        public string SortDirection { get; set; }
    }
}
