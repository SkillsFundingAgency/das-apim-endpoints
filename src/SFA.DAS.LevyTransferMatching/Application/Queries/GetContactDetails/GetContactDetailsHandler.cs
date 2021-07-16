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
            var allJobRolesTask = _referenceDataService.GetJobRoles();
            var allLevelsTask = _referenceDataService.GetLevels();
            var allSectorsTask = _referenceDataService.GetSectors();
            var pledgeTask = _levyTransferMatchingService.GetPledge(request.OpportunityId);

            await Task.WhenAll(allJobRolesTask, allLevelsTask, allSectorsTask, pledgeTask);

            var allJobRoles = allJobRolesTask.Result;
            var allLevels = allLevelsTask.Result;
            var allSectors = allSectorsTask.Result;
            var pledge = pledgeTask.Result;

            if (pledge != null)
            {
                return new GetContactDetailsResult()
                {
                    AllJobRolesCount = allJobRoles.Count,
                    AllLevelsCount = allLevels.Count,
                    AllSectorsCount = allSectors.Count,
                    PledgeAmount = pledgeTask.Result.Amount,
                    PledgeDasAccountName = pledgeTask.Result.DasAccountName,
                    PledgeJobRoles = allJobRoles.Where(x => pledge.JobRoles.Contains(x.Id)),
                    PledgeLevels = allLevels.Where(x => pledge.Levels.Contains(x.Id)),
                    PledgeSectors = allSectors.Where(x => pledge.Sectors.Contains(x.Id)),
                };
            }
            else
            {
                return null;
            }
        }
    }
}