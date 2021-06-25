using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetLocations
{
    public class GetLocationQueryHandler : IRequestHandler<GetLocationQuery, GetLocationResult>
    {
        private readonly ILocationLookupService _locationLookupService;

        public GetLocationQueryHandler(ILocationLookupService locationLookupService)
        {
            _locationLookupService = locationLookupService;
        }

        public async Task<GetLocationResult> Handle(GetLocationQuery request, CancellationToken cancellationToken)
        {
            var locationInformation = await _locationLookupService.GetLocationInformation(request.Location, request.Latitude, request.Longitude);
            return new GetLocationResult() { LocationName = locationInformation.Name };
        }
    }
}
