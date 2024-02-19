namespace SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Models
{
    public record GeoPoint
    {
        public double Longitude { get; init; }

        public double Latitude { get; init; }

        public int Easting { get; init; }

        public int Northing { get; init; }
    }
}
