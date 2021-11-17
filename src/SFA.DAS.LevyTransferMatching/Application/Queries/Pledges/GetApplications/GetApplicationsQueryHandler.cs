using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications
{
    public class GetApplicationsQueryHandler : IRequestHandler<GetApplicationsQuery, GetApplicationsQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IReferenceDataService _referenceDataService;

        public GetApplicationsQueryHandler(ILevyTransferMatchingService levyTransferMatchingService, IReferenceDataService referenceDataService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _referenceDataService = referenceDataService;
        }

        public async Task<GetApplicationsQueryResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            var applicationsResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest
            {
                PledgeId = request.PledgeId,
                AccountId = request.AccountId
            });

            var pledgeResponse = await _levyTransferMatchingService.GetPledge(request.PledgeId);
            var roleReferenceData = await _referenceDataService.GetJobRoles();

            var result = new List<GetApplicationsQueryResult.Application>();
            
            foreach (var application in applicationsResponse.Applications)
            {
                var roles = roleReferenceData.Where(x => pledgeResponse.JobRoles.Contains(x.Id));

                var applicationResult = (GetApplicationsQueryResult.Application)application;

                applicationResult.IsLocationMatch = !pledgeResponse.Locations.Any() || application.Locations.Any();
                applicationResult.IsSectorMatch = !pledgeResponse.Sectors.Any() || application.Sectors.Any(x => pledgeResponse.Sectors.Contains(x));
                applicationResult.IsJobRoleMatch = !pledgeResponse.JobRoles.Any() || roles.Any(r => r.Description == application.StandardRoute);
                applicationResult.IsLevelMatch = !pledgeResponse.Levels.Any() || pledgeResponse.Levels.Select(x => char.GetNumericValue(x.Last())).Contains(application.StandardLevel);

                result.Add(applicationResult);
            }

            return new GetApplicationsQueryResult
            {
                Applications = result
            };
        }
    }
}