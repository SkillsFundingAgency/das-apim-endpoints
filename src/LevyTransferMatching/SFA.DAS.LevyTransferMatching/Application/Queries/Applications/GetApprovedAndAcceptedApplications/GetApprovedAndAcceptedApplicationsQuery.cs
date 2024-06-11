using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApprovedAndAcceptedApplications
{
    public class GetApprovedAndAcceptedApplicationsQuery : IRequest<GetApprovedAndAcceptedApplicationsResult>
    {
        public long AccountId { get; set; }
    }
}
