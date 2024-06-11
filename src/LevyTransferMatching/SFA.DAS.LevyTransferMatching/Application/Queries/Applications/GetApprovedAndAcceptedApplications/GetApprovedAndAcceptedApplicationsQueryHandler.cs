using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApprovedAndAcceptedApplications
{
    public class GetApprovedAndAcceptedApplicationsQueryHandler : IRequestHandler<GetApprovedAndAcceptedApplicationsQuery, GetApprovedAndAcceptedApplicationsResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetApprovedAndAcceptedApplicationsQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetApprovedAndAcceptedApplicationsResult> Handle(GetApprovedAndAcceptedApplicationsQuery request, CancellationToken cancellationToken)
        {
            var approvedResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest
            {
                SenderAccountId = request.AccountId,
                ApplicationStatusFilter = ApplicationStatus.Approved
            });
            var acceptedResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest
            {
                SenderAccountId = request.AccountId,
                ApplicationStatusFilter = ApplicationStatus.Accepted
            });

            var applicationsResponse = approvedResponse.Applications.Concat(acceptedResponse.Applications);

            return new GetApprovedAndAcceptedApplicationsResult
            {
                Applications = applicationsResponse?.Select(x => (PledgeApplication)x)
            };
        }
    }
}