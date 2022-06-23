using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetGeoPoint
{
    public class GetGeoPointQueryHandler : IRequestHandler<GetGeoPointQuery, GetGeoPointQueryResult>
    {
        private readonly ILocationLookupService _locationLookupService;

        public GetGeoPointQueryHandler(ILocationLookupService locationLookupService)
        {
            _locationLookupService = locationLookupService;
        }

        public async Task<GetGeoPointQueryResult> Handle(GetGeoPointQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Postcode)) throw new ArgumentException($"Postcode is required", nameof(GetGeoPointQuery.Postcode));

            var location = await _locationLookupService.GetLocationInformation(request.Postcode, default, default);

            if (location == null)
            {
                return new GetGeoPointQueryResult(null);
            }

            return new GetGeoPointQueryResult(new GetGeoPointResponse 
            { 
                GeoPoint = new GeoPoint 
                { 
                    Postcode = location.Name, 
                    Latitude = location.GeoPoint[0], 
                    Longitude = location.GeoPoint[1] 
                } 
            });
        }
    }
}
