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
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplication
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
            var application = await _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.ApplicationId));

            if (application == null)
            {
                return null;
            }
            
            var standardListItemTask = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(application.StandardId));
            var allJobRolesTask = _referenceDataService.GetJobRoles();
            var allLevelsTask = _referenceDataService.GetLevels();
            var allSectorsTask = _referenceDataService.GetSectors();

            await Task.WhenAll(allJobRolesTask, allLevelsTask, allSectorsTask);

            var standardListItem = standardListItemTask.Result;

            var standard = new Standard()
            {
                LarsCode = standardListItem.LarsCode,
                Level = standardListItem.Level,
                StandardUId = standardListItem.StandardUId,
                Title = standardListItem.Title,
                ApprenticeshipFunding = standardListItem.ApprenticeshipFunding?.Select(funding =>
                    new ApprenticeshipFunding()
                    {
                        Duration = funding.Duration,
                        EffectiveFrom = funding.EffectiveFrom,
                        EffectiveTo = funding.EffectiveTo,
                        MaxEmployerLevyCap = funding.MaxEmployerLevyCap,
                    })
            };

            return new GetApplicationResult()
            {
                AllJobRoles = allJobRolesTask.Result,
                AllLevels = allLevelsTask.Result,
                AllSectors = allSectorsTask.Result,
                Amount = application.Amount,
                EmployerAccountName = application.EmployerAccountName,
                PledgeEmployerAccountName = application.PledgeEmployerAccountName,
                IsNamePublic = application.PledgeIsNamePublic,
                JobRoles = application.PledgeJobRoles,
                Levels = application.PledgeLevels,
                PledgeLocations = application.PledgeLocations.Select(x => x.Name),
                NumberOfApprentices = application.NumberOfApprentices,
                RemainingAmount = application.PledgeRemainingAmount,
                Sectors = application.PledgeSectors,
                StartBy = application.StartDate,
                Status = application.Status,
                OpportunityId = application.PledgeId,
                Standard = standard,
                PledgeAmount = application.PledgeAmount,
                SenderEmployerAccountId = application.SenderEmployerAccountId,
                AmountUsed = application.AmountUsed,
                NumberOfApprenticesUsed = application.NumberOfApprenticesUsed
            };
        }
    }
}