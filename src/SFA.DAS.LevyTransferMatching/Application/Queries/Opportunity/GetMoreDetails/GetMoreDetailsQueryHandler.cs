using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetMoreDetails
{
    public class GetMoreDetailsQueryHandler : IRequestHandler<GetMoreDetailsQuery, GetMoreDetailsQueryResult>
    {
        private readonly IReferenceDataService _referenceDataService;
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetMoreDetailsQueryHandler(IReferenceDataService referenceDataService, ILevyTransferMatchingService levyTransferMatchingService)
        {
            _referenceDataService = referenceDataService;
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetMoreDetailsQueryResult> Handle(GetMoreDetailsQuery request, CancellationToken cancellationToken)
        {
            var sectorsTask = _referenceDataService.GetSectors();
            var jobRolesTask = _referenceDataService.GetJobRoles();
            var levelsTask = _referenceDataService.GetLevels();
            var opportunityTask = _levyTransferMatchingService.GetPledge(request.OpportunityId);

            await Task.WhenAll(sectorsTask, jobRolesTask, levelsTask, opportunityTask);

            return new GetMoreDetailsQueryResult
            {
                Opportunity = opportunityTask.Result,
                Sectors = sectorsTask.Result,
                JobRoles = jobRolesTask.Result,
                Levels = levelsTask.Result
            };
        }
    }
}
