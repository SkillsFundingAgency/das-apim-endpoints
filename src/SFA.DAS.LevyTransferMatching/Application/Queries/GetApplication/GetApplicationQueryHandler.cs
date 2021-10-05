using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication
{
    public class GetApplicationQueryHandler : IRequestHandler<GetApplicationQuery, GetApplicationResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IReferenceDataService _referenceDataService;

        public GetApplicationQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ILevyTransferMatchingService levyTransferMatchingService, IReferenceDataService referenceDataService)
        {
            _coursesApiClient = coursesApiClient;
            _levyTransferMatchingService = levyTransferMatchingService;
            _referenceDataService = referenceDataService;
        }

        public async Task<GetApplicationResult> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
        {
            var application = await _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.PledgeId, request.ApplicationId));

            if (application == null)
            {
                return null;
            }

            var standardTask = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(application.StandardId));
            var allJobRolesTask = _referenceDataService.GetJobRoles();
            var allLevelsTask = _referenceDataService.GetLevels();
            var allSectorsTask = _referenceDataService.GetSectors();

            await Task.WhenAll(standardTask, allJobRolesTask, allLevelsTask, allSectorsTask);

            var estimatedDurationMonths = standardTask.Result.TypicalDuration;
            var level = standardTask.Result.Level;
            var typeOfJobRole = standardTask.Result.Title;

            var getApplicationResult = new GetApplicationResult()
            {
                AboutOpportunity = application.Details,
                BusinessWebsite = application.BusinessWebsite,
                EmailAddresses = application.EmailAddresses,
                EstimatedDurationMonths = estimatedDurationMonths,
                MaxFunding = standardTask.Result.MaxFunding,
                Amount = application.Amount,
                FirstName = application.FirstName,
                HasTrainingProvider = application.HasTrainingProvider,
                LastName = application.LastName,
                Level = level,
                Location = string.Empty, //replaced in TM-169
                NumberOfApprentices = application.NumberOfApprentices,
                Sector = application.Sectors,
                StartBy = application.StartDate,
                TypeOfJobRole = typeOfJobRole,
                EmployerAccountName = application.EmployerAccountName,
                PledgeSectors = application.PledgeSectors,
                PledgeLevels = application.PledgeLevels,
                PledgeJobRoles = application.PledgeJobRoles,
                PledgeLocations = application.Locations?.Select(x => x.Name),
                PledgeRemainingAmount = application.PledgeRemainingAmount,
                AllJobRoles = allJobRolesTask.Result,
                AllLevels = allLevelsTask.Result,
                AllSectors = allSectorsTask.Result,
                Status = application.Status
            };

            return getApplicationResult;
        }
    }
}