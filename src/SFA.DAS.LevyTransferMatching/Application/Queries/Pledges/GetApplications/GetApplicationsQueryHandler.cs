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
            var applicationsTask = _levyTransferMatchingService.GetApplications(new GetApplicationsRequest
            {
                PledgeId = request.PledgeId,
                SortOrder = request.SortOrder,
                SortDirection = request.SortDirection
            });

            var pledgeTask = _levyTransferMatchingService.GetPledge(request.PledgeId);
            
            await Task.WhenAll(applicationsTask, pledgeTask);

            var roleReferenceData = await _referenceDataService.GetJobRoles();
            var roles = roleReferenceData.Where(x => pledgeTask.Result.JobRoles.Contains(x.Id));

            var result = new List<GetApplicationsQueryResult.Application>();
            foreach (var application in applicationsTask.Result.Applications)
            {
                result.Add(GetApplicationsQueryResult.Application.BuildApplication(application, roles, pledgeTask.Result));
            }

            return new GetApplicationsQueryResult
            {
                Applications = result,
                PledgeStatus = pledgeTask.Result.Status
            };
        }
        
    }
}