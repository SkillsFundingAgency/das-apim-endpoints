using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerDemand.Application.Locations.Queries.GetLocations;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerDemand.Api.Models
{
    public class GetLocationSearchResponse
    {
        public IEnumerable<GetLocationSearchResponseItem> Locations { get; set; }
        public static implicit operator GetLocationSearchResponse(GetLocationsQueryResponse source)
        {
            return new GetLocationSearchResponse
            {
                Locations = source.Locations.Select(c => (GetLocationSearchResponseItem)c).ToList(),
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

        public static implicit operator GetLocationSearchResponseItem(LocationItem source)
        {
            if (source == null)
            {
                return null;
            }
            
            return new GetLocationSearchResponseItem
            {
                Name = source.Name,
                Location = new LocationResponse
                {
                    GeoPoint = source.GeoPoint
                }
            };
        }

        public static implicit operator GetLocationSearchResponseItem(Location source)
        {
            if (source == null)
            {
                return null;
            }
            
            return new GetLocationSearchResponseItem
            {
                Name = source.Name,
                Location = new LocationResponse
                {
                    GeoPoint = source.LocationPoint.GeoPoint.ToArray()
                }
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
