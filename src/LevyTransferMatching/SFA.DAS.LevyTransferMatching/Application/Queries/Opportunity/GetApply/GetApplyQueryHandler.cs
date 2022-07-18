using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models.Constants;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetApply
{
    public class GetApplyQueryHandler : IRequestHandler<GetApplyQuery, GetApplyQueryResult>
    {
        private readonly IReferenceDataService _referenceDataService;
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetApplyQueryHandler(IReferenceDataService referenceDataService, ILevyTransferMatchingService levyTransferMatchingService)
        {
            _referenceDataService = referenceDataService;
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetApplyQueryResult> Handle(GetApplyQuery request, CancellationToken cancellationToken)
        {
            var sectorsTask = _referenceDataService.GetSectors();
            var jobRolesTask = _referenceDataService.GetJobRoles();
            var levelsTask = _referenceDataService.GetLevels();
            var opportunityTask = _levyTransferMatchingService.GetPledge(request.OpportunityId);

            await Task.WhenAll(sectorsTask, jobRolesTask, levelsTask, opportunityTask);

            if (opportunityTask.Result.Status == PledgeStatus.Closed)
            {
                return null;
            }

            return new GetApplyQueryResult
            {
                Opportunity = opportunityTask.Result,
                Sectors = sectorsTask.Result,
                JobRoles = jobRolesTask.Result,
                Levels = levelsTask.Result
            };
        }
    }
}
