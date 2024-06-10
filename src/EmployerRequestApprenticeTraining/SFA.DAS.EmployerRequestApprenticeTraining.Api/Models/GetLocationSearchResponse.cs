﻿using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.Models
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
