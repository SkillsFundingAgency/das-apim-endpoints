using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetApplicationDetails
{
    public class GetApplicationDetailsQueryHandler : IRequestHandler<GetApplicationDetailsQuery, GetApplicationDetailsQueryResult>
    {
        private readonly IReferenceDataService _referenceDataService;
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetApplicationDetailsQueryHandler(IReferenceDataService referenceDataService, ILevyTransferMatchingService levyTransferMatchingService, ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _referenceDataService = referenceDataService;
            _levyTransferMatchingService = levyTransferMatchingService;
            _coursesApiClient = coursesApiClient;
        }

        public async Task<GetApplicationDetailsQueryResult> Handle(GetApplicationDetailsQuery request, CancellationToken cancellationToken)
        {
            var sectorsTask = _referenceDataService.GetSectors();
            var jobRolesTask = _referenceDataService.GetJobRoles();
            var levelsTask = _referenceDataService.GetLevels();
            var opportunityTask = _levyTransferMatchingService.GetPledge(request.OpportunityId);
            var standardsTask = GetStandardsList(request.StandardId);

            await Task.WhenAll(sectorsTask, jobRolesTask, levelsTask, opportunityTask, standardsTask);

            return new GetApplicationDetailsQueryResult
            {
                Opportunity = opportunityTask.Result,
                Standards = standardsTask.Result,
                Sectors = sectorsTask.Result,
                JobRoles = jobRolesTask.Result,
                Levels = levelsTask.Result
            };
        }

        private async Task<List<GetStandardsListItem>> GetStandardsList(string standardId)
        {
            var standards = new List<GetStandardsListItem>();

            if (string.IsNullOrEmpty(standardId))
            {
                var standardsResponse = await _coursesApiClient.Get<GetStandardsListResponse>(new GetAvailableToStartStandardsListRequest());
                standards = standardsResponse.Standards.ToList();
            }

            else
            {
                var standard = await _coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(standardId));
                standards.Add(standard);
            }

            return standards;
        }
    }
}
