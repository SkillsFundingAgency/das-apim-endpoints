using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;

public sealed class GetCourseTrainingProvidersCountRequest : IGetApiRequest
{
    public string[] LarsCodes { get; }

    public int? Distance { get; }

    public decimal? Latitude { get; }

    public decimal? Longitude { get; }

    public GetCourseTrainingProvidersCountRequest(string[] larsCodes, int? distance = null, decimal? latitude = null, decimal? longitude = null)
    {
        LarsCodes = larsCodes;
        Distance = distance;
        Latitude = latitude;
        Longitude = longitude;
    }

    private const string PROVIDERS_COUNT_URL = "courses/providers-count";

    public string GetUrl => BuildUrl();

    private string BuildUrl()
    {
        var queryParameters = new List<string>();

        if (LarsCodes != null && LarsCodes.Any())
        {
            queryParameters.Add("larsCodes=" + string.Join("&larsCodes=", LarsCodes));
        }

        if (Distance != null)
        {
            queryParameters.Add($"distance={Distance}");
        }

        if (Latitude != null)
        {
            queryParameters.Add($"latitude={Latitude}");
        }

        if (Longitude != null)
        {
            queryParameters.Add($"longitude={Longitude}");
        }

        var queryString = string.Join("&", queryParameters);

        return queryParameters.Any() ? $"{PROVIDERS_COUNT_URL}?{queryString}" : PROVIDERS_COUNT_URL;
    }
}