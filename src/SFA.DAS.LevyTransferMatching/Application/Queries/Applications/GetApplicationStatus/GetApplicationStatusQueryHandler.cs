using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplicationStatus
{
    public class GetApplicationStatusQueryHandler : IRequestHandler<GetApplicationStatusQuery, GetApplicationStatusResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IReferenceDataService _referenceDataService;

        public GetApplicationStatusQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ILevyTransferMatchingService levyTransferMatchingService, IReferenceDataService referenceDataService)
        {
            _coursesApiClient = coursesApiClient;
            _levyTransferMatchingService = levyTransferMatchingService;
            _referenceDataService = referenceDataService;
        }

        public async Task<GetApplicationStatusResult> Handle(GetApplicationStatusQuery request, CancellationToken cancellationToken)
        {
            var application = await _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.OpportunityId, request.ApplicationId));

            if (application == null)
            {
                return null;
            }

            var standardTask = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(application.StandardId));
            var allJobRolesTask = _referenceDataService.GetJobRoles();
            var allLevelsTask = _referenceDataService.GetLevels();
            var allSectorsTask = _referenceDataService.GetSectors();

            await Task.WhenAll(allJobRolesTask, allLevelsTask, allSectorsTask);

            var level = standardTask.Result.Level;
            var jobRole = standardTask.Result.Title;

            return new GetApplicationStatusResult()
            {
                AllJobRoles = allJobRolesTask.Result,
                AllLevels = allLevelsTask.Result,
                AllSectors = allSectorsTask.Result,
                Amount = application.Amount,
                EmployerAccountName = application.EmployerAccountName,
                IsNamePublic = application.PledgeIsNamePublic,
                JobRole = jobRole,
                JobRoles = application.PledgeJobRoles,
                Level = level,
                Levels = application.PledgeLevels,
                Locations = application.Locations?.Select(x => x.Name),
                NumberOfApprentices = application.NumberOfApprentices,
                RemainingAmount = application.PledgeRemainingAmount,
                Sectors = application.Sectors,
                StartBy = application.StartDate,
                Status = application.Status,
            };
        }
    }
}