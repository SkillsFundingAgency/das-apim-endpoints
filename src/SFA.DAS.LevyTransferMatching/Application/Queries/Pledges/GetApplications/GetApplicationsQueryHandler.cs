using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

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
                
                AddPledgeDetailsToApplication(applicationResult, pledgeResponse, application, roles);
                result.Add(applicationResult);
            }

            return new GetApplicationsQueryResult
            {
                Applications = result
            };
        }

        private static void AddPledgeDetailsToApplication(GetApplicationsQueryResult.Application applicationResult, Pledge pledgeResponse,
            GetApplicationsResponse.Application application, IEnumerable<ReferenceDataItem> roles)
        {
            applicationResult.JobRole = application.StandardTitle;
            applicationResult.FirstName = application.FirstName;
            applicationResult.LastName = application.LastName;
            applicationResult.EmployerAccountName = pledgeResponse.DasAccountName;
            applicationResult.PledgeRemainingAmount = pledgeResponse.RemainingAmount;
            applicationResult.IsLocationMatch = !pledgeResponse.Locations.Any() || application.Locations.Any();
            applicationResult.IsSectorMatch = !pledgeResponse.Sectors.Any() || application.Sectors.Any(x => pledgeResponse.Sectors.Contains(x));
            applicationResult.IsJobRoleMatch = !pledgeResponse.JobRoles.Any() || roles.Any(r => r.Description == application.StandardRoute);
            applicationResult.IsLevelMatch = !pledgeResponse.Levels.Any() || pledgeResponse.Levels.Select(x => char.GetNumericValue(x.Last())).Contains(application.StandardLevel);
            applicationResult.PledgeLocations = pledgeResponse.Locations;
        }
    }
}