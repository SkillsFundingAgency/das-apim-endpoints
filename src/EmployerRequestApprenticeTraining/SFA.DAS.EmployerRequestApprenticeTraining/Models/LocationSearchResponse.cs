using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Models
{
    public class LocationSearchResponse
    {
        public LocationCoordinates Coordinates { get; set; }
        public string Name { get; set; }

        public static implicit operator LocationSearchResponse(GetLocationsListItem source)
        {
            return new LocationSearchResponse
            {
                Name = source.DisplayName,
                Coordinates = source.Location,
            };
        }

        public class LocationCoordinates
        {
            public double[] GeoPoint { get; set; }

            public static implicit operator LocationCoordinates(GetLocationsListItem.Coordinates source)
            {
                return new LocationCoordinates
                {
                    GeoPoint = source.GeoPoint
                };
            }
        }
    }
}
