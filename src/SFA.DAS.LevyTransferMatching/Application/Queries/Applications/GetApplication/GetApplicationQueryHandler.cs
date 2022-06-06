using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.CommitmentsV2;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses.CommitmentsV2;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplication
{
    public class GetApplicationQueryHandler : IRequestHandler<GetApplicationQuery, GetApplicationResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IReferenceDataService _referenceDataService;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;

        public GetApplicationQueryHandler(ILevyTransferMatchingService levyTransferMatchingService, IReferenceDataService referenceDataService, ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _referenceDataService = referenceDataService;
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
        }

        public async Task<GetApplicationResult> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
        {
            var application = await _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.ApplicationId));

            if (application == null)
            {
                return null;
            }
            
            var allJobRolesTask = _referenceDataService.GetJobRoles();
            var allLevelsTask = _referenceDataService.GetLevels();
            var allSectorsTask = _referenceDataService.GetSectors();
            var pledgeTask = _levyTransferMatchingService.GetPledge(application.PledgeId);
            var cohortTask = _commitmentsV2ApiClient.Get<GetCohortsResponse>(new GetCohortsRequest { AccountId = request.AccountId });

            await Task.WhenAll(allJobRolesTask, allLevelsTask, allSectorsTask, pledgeTask, cohortTask);

            var IsWithdrawableAfterAcceptance = !cohortTask.Result.Cohorts.Any(x => x.PledgeApplicationId.HasValue && x.PledgeApplicationId == request.ApplicationId) &&
                                                    application.NumberOfApprenticesUsed == 0;

            return new GetApplicationResult
            {
                StandardTitle = application.StandardTitle,
                StandardLevel = application.StandardLevel,
                StandardDuration = application.StandardDuration,
                StandardMaxFunding = application.StandardMaxFunding,
                AllJobRoles = allJobRolesTask.Result,
                AllLevels = allLevelsTask.Result,
                AllSectors = allSectorsTask.Result,
                Amount = application.TotalAmount,
                TotalAmount = application.TotalAmount,
                EmployerAccountName = application.EmployerAccountName,
                PledgeEmployerAccountName = pledgeTask.Result.DasAccountName,
                IsNamePublic = pledgeTask.Result.IsNamePublic,
                JobRoles = pledgeTask.Result.JobRoles,
                Levels = pledgeTask.Result.Levels,
                PledgeLocations = pledgeTask.Result.Locations.Select(x => x.Name),
                NumberOfApprentices = application.NumberOfApprentices,
                RemainingAmount = pledgeTask.Result.RemainingAmount,
                Sectors = pledgeTask.Result.Sectors,
                StartBy = application.StartDate,
                Status = application.Status,
                OpportunityId = application.PledgeId,
                PledgeAmount = pledgeTask.Result.Amount,
                SenderEmployerAccountId = application.SenderEmployerAccountId,
                AmountUsed = application.AmountUsed,
                NumberOfApprenticesUsed = application.NumberOfApprenticesUsed,
                AutomaticApproval = application.AutomaticApproval,
                IsWithdrawableAfterAcceptance = IsWithdrawableAfterAcceptance
            };
        }
    }
}