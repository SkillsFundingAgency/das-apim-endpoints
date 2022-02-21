using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models.Constants;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationsAccountNames
{
    public class GetApplicationsAccountNamesQueryHandler : IRequestHandler<GetApplicationsAccountNamesQuery, GetApplicationsAccountNamesQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetApplicationsAccountNamesQueryHandler(ILevyTransferMatchingService levyTransferMatchingService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetApplicationsAccountNamesQueryResult> Handle(GetApplicationsAccountNamesQuery request, CancellationToken cancellationToken)
        {
            var applicationsResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest 
            {
                PledgeId = request.PledgeId,
                ApplicationStatusFilter = PledgeStatus.Pending 
            });

            return new GetApplicationsAccountNamesQueryResult
            {
                Applications = applicationsResponse?.Applications.Select(x => new GetApplicationsAccountNamesQueryResult.Application
                {
                    Id = x.Id,
                    DasAccountName = x.DasAccountName,
                })
            };
        }
    }
}
