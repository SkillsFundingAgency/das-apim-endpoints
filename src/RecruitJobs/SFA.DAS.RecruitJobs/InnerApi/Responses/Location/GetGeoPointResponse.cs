namespace SFA.DAS.RecruitJobs.InnerApi.Responses.Location;

public class GetGeoPointResponse
{
    public GeoPoint GeoPoint { get; set; }
}

public class GeoPoint
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}