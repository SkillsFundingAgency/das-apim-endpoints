using MediatR;
using SFA.DAS.LevyTransferMatching.Infrastructure;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications
{
    public class GetApplicationsQuery : PagedQuery, IRequest<GetApplicationsQueryResult>
    {
        public long AccountId { get; set; }
    }
}
