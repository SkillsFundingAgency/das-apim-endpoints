using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Models.Constants;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetDetail
{
    public class GetDetailQueryHandler : IRequestHandler<GetDetailQuery, GetDetailQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IReferenceDataService _referenceDataService;

        public GetDetailQueryHandler(ILevyTransferMatchingService levyTransferMatchingService, IReferenceDataService referenceDataService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _referenceDataService = referenceDataService;
        }

        public async Task<GetDetailQueryResult> Handle(GetDetailQuery request, CancellationToken cancellationToken)
        {
            var opportunityTask = _levyTransferMatchingService.GetPledge(request.OpportunityId);
            var sectorsTask = _referenceDataService.GetSectors();
            var jobRolesTask = _referenceDataService.GetJobRoles();
            var levelsTask = _referenceDataService.GetLevels();

            await Task.WhenAll(opportunityTask, sectorsTask, jobRolesTask, levelsTask);

            if (opportunityTask.Result.Status == PledgeStatus.Closed)
            {
                return null;
            }

            return new GetDetailQueryResult
            {
                Opportunity = opportunityTask.Result,
                Sectors = sectorsTask.Result,
                JobRoles = jobRolesTask.Result,
                Levels = levelsTask.Result
            };
        }
    }
}
