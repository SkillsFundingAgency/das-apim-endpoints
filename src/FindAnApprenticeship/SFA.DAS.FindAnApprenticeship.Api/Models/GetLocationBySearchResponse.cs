using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetLocationsBySearch;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class GetLocationBySearchResponse
    {
        public IEnumerable<GetLocationSearchResponseItem> Locations { get; set; }

        public static implicit operator GetLocationBySearchResponse(GetLocationsBySearchQueryResult source)
        {
            return new GetLocationBySearchResponse 
            { 
                Locations = source.Locations.Select(l => (GetLocationSearchResponseItem)l).ToList()
            };
        }
    }
    public class GetLocationSearchResponseItem
    {
        public LocationResponse Location { get; set; }
        public string Name { get; set; }

        public static implicit operator GetLocationSearchResponseItem(GetLocationsListItem source)
        {
            return new GetLocationSearchResponseItem
            {
                Name = source.DisplayName,
                Location = source.Location,
            };
        }

        public class LocationResponse
        {
            public double[] GeoPoint { get; set; }

            public static implicit operator LocationResponse(GetLocationsListItem.Coordinates source)
            {
                return new LocationResponse
                {
                    GeoPoint = source.GeoPoint
                };
            }
        }
    }
}
