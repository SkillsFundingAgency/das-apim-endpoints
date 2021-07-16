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

        public GetSectorQueryHandler(IReferenceDataService referenceDataService, ILocationLookupService locationLookupService)
        {
            _referenceDataService = referenceDataService;
            _locationLookupService = locationLookupService;
        }

        public async Task<GetSectorQueryResult> Handle(GetSectorQuery request, CancellationToken cancellationToken)
        {
            var locationInformation = await _locationLookupService.GetLocationInformation(request.Postcode, 0, 0, true);
            var sectors = await _referenceDataService.GetSectors();

            return new GetSectorQueryResult
            {
                Sectors = sectors,
                Location = locationInformation?.Name
            };
        }
    }
}
