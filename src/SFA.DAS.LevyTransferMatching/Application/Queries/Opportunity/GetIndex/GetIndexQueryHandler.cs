using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.Models.Constants;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetIndex
{
    public class GetIndexQueryHandler : IRequestHandler<GetIndexQuery, GetIndexQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IReferenceDataService _referenceDataService;

        public GetIndexQueryHandler(ILevyTransferMatchingService levyTransferMatchingService, IReferenceDataService referenceDataService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _referenceDataService = referenceDataService;
        }

        public async Task<GetIndexQueryResult> Handle(GetIndexQuery request, CancellationToken cancellationToken)
        {
            var opportunitiesTask = _levyTransferMatchingService.GetPledges(new GetPledgesRequest(null, request.Sectors));
            var sectorsTask = _referenceDataService.GetSectors();
            var jobRolesTask = _referenceDataService.GetJobRoles();
            var levelsTask = _referenceDataService.GetLevels();

            await Task.WhenAll(opportunitiesTask, sectorsTask, jobRolesTask, levelsTask);

            return new GetIndexQueryResult
            {
                Opportunities = opportunitiesTask.Result.Pledges.Where(p => p.Status != PledgeStatus.Closed).Select(x => new GetIndexQueryResult.Opportunity
                {
                    Id = x.Id,
                    Amount = x.RemainingAmount,
                    IsNamePublic = x.IsNamePublic,
                    DasAccountName = x.DasAccountName,
                    Sectors = x.Sectors,
                    JobRoles = x.JobRoles,
                    Levels = x.Levels,
                    Locations = x.Locations?.Select(x => x.Name)
                }),
                Sectors = sectorsTask.Result,
                JobRoles = jobRolesTask.Result,
                Levels = levelsTask.Result
            };
        }
    }
}
