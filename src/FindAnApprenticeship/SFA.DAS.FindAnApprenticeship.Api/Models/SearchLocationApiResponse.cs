using System.Linq;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models;

public class SearchLocationApiResponse
{
    public static implicit operator SearchLocationApiResponse(LocationItem source)
    {
        if (source == null)
        {
            return null;
        }
        return new SearchLocationApiResponse
        {
            Lat = source.GeoPoint.First(),
            Lon = source.GeoPoint.Last(),
            LocationName = source.Name
        };
    }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string LocationName { get; set; }
}