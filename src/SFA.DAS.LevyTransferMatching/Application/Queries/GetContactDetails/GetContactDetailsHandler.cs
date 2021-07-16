using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetContactDetails
{
    public class GetContactDetailsHandler : IRequestHandler<GetContactDetailsQuery, GetContactDetailsResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IReferenceDataService _referenceDataService;

        public GetContactDetailsHandler(ILevyTransferMatchingService levyTransferMatchingService, IReferenceDataService referenceDataService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _referenceDataService = referenceDataService;
        }

        public async Task<GetContactDetailsResult> Handle(GetContactDetailsQuery request, CancellationToken cancellationToken)
        {
            var pledge = await _levyTransferMatchingService.GetPledge(request.OpportunityId);

            if (pledge != null)
            {
                var allJobRolesTask = _referenceDataService.GetJobRoles();
                var allLevelsTask = _referenceDataService.GetLevels();
                var allSectorsTask = _referenceDataService.GetSectors();

                await Task.WhenAll(allJobRolesTask, allLevelsTask, allSectorsTask);

                var allJobRoles = allJobRolesTask.Result;
                var allLevels = allLevelsTask.Result;
                var allSectors = allSectorsTask.Result;

                return new GetContactDetailsResult()
                {
                    AllJobRolesCount = allJobRoles.Count,
                    AllLevelsCount = allLevels.Count,
                    AllSectorsCount = allSectors.Count,
                    OpportunityAmount = pledge.Amount,
                    OpportunityDasAccountName = pledge.DasAccountName,
                    OpportunityJobRoleDescriptions = allJobRoles.Where(x => pledge.JobRoles.Contains(x.Id)).Select(x => x.Description),
                    OpportunityLevelDescriptions = allLevels.Where(x => pledge.Levels.Contains(x.Id)).Select(x => x.ShortDescription), // Note: levels here are different - the ShortDescription is used
                    OpportunitySectorDescriptions = allSectors.Where(x => pledge.Sectors.Contains(x.Id)).Select(x => x.Description),
                    OpportunityIsNamePublic = pledge.IsNamePublic,
                };
            }
            else
            {
                return null;
            }
        }
    }
}