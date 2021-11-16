using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplication
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
            var application = await _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.ApplicationId));

            if (application == null)
            {
                return null;
            }
            
            var allJobRolesTask = _referenceDataService.GetJobRoles();
            var allLevelsTask = _referenceDataService.GetLevels();
            var allSectorsTask = _referenceDataService.GetSectors();

            await Task.WhenAll(allJobRolesTask, allLevelsTask, allSectorsTask);
            
            return new GetApplicationResult
            {
                StandardTitle = application.StandardTitle,
                StandardLevel = application.StandardLevel,
                StandardDuration = application.StandardDuration,
                StandardMaxFunding = application.StandardMaxFunding,
                AllJobRoles = allJobRolesTask.Result,
                AllLevels = allLevelsTask.Result,
                AllSectors = allSectorsTask.Result,
                Amount = application.Amount,
                TotalAmount = application.TotalAmount,
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
                PledgeAmount = application.PledgeAmount,
                SenderEmployerAccountId = application.SenderEmployerAccountId,
                AmountUsed = application.AmountUsed,
                NumberOfApprenticesUsed = application.NumberOfApprenticesUsed,
                AllowTransferRequestAutoApproval = application.AllowTransferRequestAutoApproval
            };
        }
    }
}