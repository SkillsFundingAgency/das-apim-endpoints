using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetSector
{
    public class GetSectorQueryHandler : IRequestHandler<GetSectorQuery, GetSectorQueryResult>
    {
        private readonly IReferenceDataService _referenceDataService;
        private readonly ILocationLookupService _locationLookupService;
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;

        public GetSectorQueryHandler(IReferenceDataService referenceDataService, ILocationLookupService locationLookupService, ILevyTransferMatchingService levyTransferMatchingService)
        {
            _referenceDataService = referenceDataService;
            _locationLookupService = locationLookupService;
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<GetSectorQueryResult> Handle(GetSectorQuery request, CancellationToken cancellationToken)
        {
            var locationInformationTask = _locationLookupService.GetLocationInformation(request.Postcode, 0, 0, true);
            var opportunityTask = _levyTransferMatchingService.GetPledge(request.OpportunityId);
            var sectorsTask = _referenceDataService.GetSectors();
            var jobRolesTask = _referenceDataService.GetJobRoles();
            var levelsTask = _referenceDataService.GetLevels();

            await Task.WhenAll(locationInformationTask, opportunityTask, sectorsTask, jobRolesTask, levelsTask);

            return new GetSectorQueryResult
            {
                Location = locationInformationTask.Result?.Name,
                Opportunity = opportunityTask.Result,
                Sectors = sectorsTask.Result,
                JobRoles = jobRolesTask.Result,
                Levels = levelsTask.Result
            };
        }
    }
}