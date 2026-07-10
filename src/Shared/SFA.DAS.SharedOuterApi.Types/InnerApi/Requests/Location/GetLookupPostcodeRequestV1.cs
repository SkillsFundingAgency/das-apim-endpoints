using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;

public class GetLookupPostcodeRequestV1(string fullPostcode) : IGetApiRequest
{
    private const string BaseUrl = "api/postcodes";

    public string GetUrl
    {
        get
        {
            var query = new Dictionary<string, string> { { "postcode", fullPostcode } };
            return QueryHelpers.AddQueryString(BaseUrl, query);
        }
    }

    public string Version => "1.0";
}