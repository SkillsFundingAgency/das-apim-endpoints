using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplicationsByStatus
{
    public class GetApplicationsByStatusQueryHandler : IRequestHandler<GetApplicationsByStatusQuery, GetApplicationsByStatusResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetApplicationsByStatusQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetApplicationsByStatusResult> Handle(GetApplicationsByStatusQuery request, CancellationToken cancellationToken)
        {
            var applicationsResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest
            {
                SenderAccountId = request.AccountId,
                ApplicationStatusFilter = request.ApplicationStatus
            });

            return new GetApplicationsByStatusResult
            {
                Applications = applicationsResponse?.Applications.Select(x => (GetApplicationsByStatusResult.Application)x)
            };
        }
    }
}