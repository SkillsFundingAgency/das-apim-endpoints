using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication
{
    public class GetApplicationQueryHandler : IRequestHandler<GetApplicationQuery, GetApplicationResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IReferenceDataService _referenceDataService;

        public GetApplicationQueryHandler(ILevyTransferMatchingService levyTransferMatchingService, IReferenceDataService referenceDataService)
        {
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

            var allJobRolesTask = _referenceDataService.GetJobRoles();
            var allLevelsTask = _referenceDataService.GetLevels();
            var allSectorsTask = _referenceDataService.GetSectors();

            var pledgeTask = _levyTransferMatchingService.GetPledge(application.PledgeId);

            await Task.WhenAll(allJobRolesTask, allLevelsTask, allSectorsTask, pledgeTask);

            var getApplicationResult = new GetApplicationResult()
            {
                AboutOpportunity = application.Details,
                BusinessWebsite = application.BusinessWebsite,
                EmailAddresses = application.EmailAddresses,
                CreatedOn = application.CreatedOn,
                EstimatedDurationMonths = application.StandardDuration,
                MaxFunding = application.StandardMaxFunding,
                Amount = application.Amount,
                FirstName = application.FirstName,
                HasTrainingProvider = application.HasTrainingProvider,
                MatchJobRole = application.MatchJobRole,
                MatchLocation = application.MatchLocation,
                MatchLevel = application.MatchLevel,
                MatchSector = application.MatchSector,
                MatchPercentage = application.MatchPercentage,
                LastName = application.LastName,
                Level = application.StandardLevel,
                Location = string.Empty, //replaced in TM-169
                NumberOfApprentices = application.NumberOfApprentices,
                Sector = application.Sectors,
                StartBy = application.StartDate,
                TypeOfJobRole = application.StandardTitle,
                EmployerAccountName = application.EmployerAccountName,                
                SenderEmployerAccountName = application.SenderEmployerAccountName,
                Locations = pledgeTask.Result.Locations?.Where(x => application.Locations.Select(y => y.PledgeLocationId).Contains(x.Id)).Select(x => x.Name),
                AdditionalLocation = application.AdditionalLocation,
                SpecificLocation = application.SpecificLocation,
                PledgeSectors = pledgeTask.Result.Sectors,
                PledgeLevels = pledgeTask.Result.Levels,
                PledgeJobRoles = pledgeTask.Result.JobRoles,
                PledgeLocations = pledgeTask.Result.Locations?.Select(x => x.Name),
                PledgeRemainingAmount = pledgeTask.Result.RemainingAmount,
                AllJobRoles = allJobRolesTask.Result,
                AllLevels = allLevelsTask.Result,
                AllSectors = allSectorsTask.Result,
                Status = application.Status,
                AutomaticApproval = application.AutomaticApproval
            };

            return getApplicationResult;
        }
    }
}