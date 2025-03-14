using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;

public class GetRoatpProvidersRequest : IGetApiRequest
{
    public bool? Live { get; set; } = false;

    public string GetUrl => BuildQuery();

    private string BuildQuery()
    {
        var queryParams = new Dictionary<string, string>();

        if (Live.HasValue)
        {
            queryParams["Live"] = Live.Value.ToString().ToLower();
        }

        return QueryHelpers.AddQueryString("api/providers", queryParams);
    }
}