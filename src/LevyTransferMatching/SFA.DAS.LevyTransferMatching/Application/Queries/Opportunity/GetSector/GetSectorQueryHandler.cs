using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetSector
{
    public class GetSectorQueryHandler : IRequestHandler<GetSectorQuery, GetSectorQueryResult>
    {
        private readonly IReferenceDataService _referenceDataService;
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetSectorQueryHandler(IReferenceDataService referenceDataService, ILevyTransferMatchingService levyTransferMatchingService)
        {
            _referenceDataService = referenceDataService;
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetSectorQueryResult> Handle(GetSectorQuery request, CancellationToken cancellationToken)
        {
            var opportunityTask = _levyTransferMatchingService.GetPledge(request.OpportunityId);
            var sectorsTask = _referenceDataService.GetSectors();
            var jobRolesTask = _referenceDataService.GetJobRoles();
            var levelsTask = _referenceDataService.GetLevels();

            await Task.WhenAll(opportunityTask, sectorsTask, jobRolesTask, levelsTask);

            return new GetSectorQueryResult
            {
                Opportunity = opportunityTask.Result,
                Sectors = sectorsTask.Result,
                JobRoles = jobRolesTask.Result,
                Levels = levelsTask.Result
            };
        }
    }
}