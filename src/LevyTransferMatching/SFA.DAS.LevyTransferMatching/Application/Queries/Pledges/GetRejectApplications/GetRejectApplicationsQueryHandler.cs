using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models.Constants;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetRejectApplications
{
    public class GetRejectApplicationsQueryHandler : IRequestHandler<GetRejectApplicationsQuery, GetRejectApplicationsQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetRejectApplicationsQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetRejectApplicationsQueryResult> Handle(GetRejectApplicationsQuery request, CancellationToken cancellationToken)
        {
            var applicationsResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest 
            {
                PledgeId = request.PledgeId,
                ApplicationStatusFilter = PledgeStatus.Pending 
            });

            return new GetRejectApplicationsQueryResult
            {
                Applications = applicationsResponse?.Applications.Select(x => new GetRejectApplicationsQueryResult.Application
                {
                    Id = x.Id,
                    DasAccountName = x.DasAccountName,
                }).OrderBy(x => x.DasAccountName).ToList()
            };
        }
    }
}
