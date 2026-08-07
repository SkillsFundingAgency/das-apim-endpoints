using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.GetGeoPoint
{
    public class GetGeoPointQueryHandler(ILocationLookupService locationLookupService)
        : IRequestHandler<GetGeoPointQuery, GetGeoPointQueryResult>
    {
        public async Task<GetGeoPointQueryResult> Handle(GetGeoPointQuery request, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(request.Postcode);

            var location = await locationLookupService.GetLocationInformation(request.Postcode, 0, 0);

            if(location is not {Country: nameof(Country.England)})
            {
                return new GetGeoPointQueryResult(null);
            }

            return new GetGeoPointQueryResult(new GetGeoPointResponse
            {
                GeoPoint = new GeoPoint
                {
                    Latitude = location.GeoPoint[0],
                    Longitude = location.GeoPoint[1]
                }
            });
        }
    }
}