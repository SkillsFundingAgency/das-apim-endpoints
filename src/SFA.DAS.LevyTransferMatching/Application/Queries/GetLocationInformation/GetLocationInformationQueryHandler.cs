using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetLocationInformation
{
    public class GetLocationInformationQueryHandler : IRequestHandler<GetLocationInformationQuery, GetLocationInformationResult>
    {
        private readonly ILocationLookupService _locationLookupService;

        public GetLocationInformationQueryHandler(ILocationLookupService locationLookupService)
        {
            _locationLookupService = locationLookupService;
        }

        public async Task<GetLocationInformationResult> Handle(GetLocationInformationQuery request, CancellationToken cancellationToken)
        {
            var result = await _locationLookupService.GetLocationInformation(request.Location, request.Latitude, request.Longitude);

            return result == null ? new GetLocationInformationResult() :
                new GetLocationInformationResult { Name = result.Name, GeoPoint = result.GeoPoint };
        }
    }
}
