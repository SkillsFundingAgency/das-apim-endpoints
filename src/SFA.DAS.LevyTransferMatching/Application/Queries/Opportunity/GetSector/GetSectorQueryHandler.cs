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
            var sectorsTask = _referenceDataService.GetSectors();
            var opportunityTask = _levyTransferMatchingService.GetPledge(request.OpportunityId);

            await Task.WhenAll(locationInformationTask, sectorsTask, opportunityTask);

            return new GetSectorQueryResult
            {
                Sectors = sectorsTask.Result,
                Location = locationInformationTask.Result?.Name,
                Opportunity = opportunityTask.Result
            };
        }
    }
}
