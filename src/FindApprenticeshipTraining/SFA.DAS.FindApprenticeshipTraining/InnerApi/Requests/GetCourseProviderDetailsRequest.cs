using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Web;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

public sealed class GetCourseProviderDetailsRequest : IGetApiRequest
{
    private int LarsCode { get; }
    private long Ukprn { get; }
    private string Location { get; }
    private decimal? Longitude { get; }
    private decimal? Latitude { get; }
    private Guid? ShortlistUserId { get; }

    public string GetUrl => BuildUrl();

    public GetCourseProviderDetailsRequest(int larsCode, long ukprn, string location, decimal? longitude, decimal? latitude, Guid? shortlistUserId)
    {
        LarsCode = larsCode;
        Ukprn = ukprn;
        Location = location;
        Longitude = longitude;
        Latitude = latitude;
        ShortlistUserId = shortlistUserId;
    }

    private string BuildUrl()
    {
        var query = HttpUtility.ParseQueryString(string.Empty);

        if (!string.IsNullOrWhiteSpace(Location))
        {
            query["location"] = Location;
        }

        if (Longitude.HasValue)
        {
            query["longitude"] = Longitude.Value.ToString();
        }

        if (Latitude.HasValue)
        {
            query["latitude"] = Latitude.Value.ToString();
        }

        if (ShortlistUserId.HasValue)
        {
            query["shortlistUserId"] = ShortlistUserId.Value.ToString();
        }

        var queryString = query.ToString();
        var url = $"api/courses/{LarsCode}/providers/{Ukprn}/details";

        return string.IsNullOrEmpty(queryString) ? url : $"{url}?{queryString}";
    }
}
