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
            
            var result = (
                from application in applicationsResponse.Applications
                let roles = roleReferenceData.Where(x => pledgeResponse.JobRoles.Contains(x.Id))
                select GetApplicationsQueryResult.Application.BuildApplication(application, roles, pledgeResponse)).ToList();
 
            return new GetApplicationsQueryResult
            {
                Applications = result
            };
        }
        
    }
}