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

            if (pledge == null)
            {
                return null;
            }

            var allJobRolesTask = _referenceDataService.GetJobRoles();
            var allLevelsTask = _referenceDataService.GetLevels();
            var allSectorsTask = _referenceDataService.GetSectors();

            await Task.WhenAll(allJobRolesTask, allLevelsTask, allSectorsTask);

            var allJobRoles = allJobRolesTask.Result;
            var allLevels = allLevelsTask.Result;
            var allSectors = allSectorsTask.Result;

            return new GetContactDetailsResult()
            {
                AllJobRoles = allJobRoles,
                AllLevels = allLevels,
                AllSectors = allSectors,
                Amount = pledge.Amount,
                DasAccountName = pledge.DasAccountName,
                JobRoles = allJobRoles.Where(x => pledge.JobRoles.Contains(x.Id)),
                Levels = allLevels.Where(x => pledge.Levels.Contains(x.Id)),
                Sectors = allSectors.Where(x => pledge.Sectors.Contains(x.Id)),
                IsNamePublic = pledge.IsNamePublic,
            };
        }
    }
}