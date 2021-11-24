using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications
{
    public class GetApplicationsQuery : IRequest<GetApplicationsQueryResult>
    {
        public long AccountId { get; set; }
    }
}
