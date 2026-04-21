using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;

public class GetLocationByFullPostcodeRequestV2(string postcode) : IGetApiRequest
{
    public string Version => "2.0";
    public string GetUrl => QueryHelpers.AddQueryString("api/postcodes", new Dictionary<string, string> { ["postcode"] = postcode });
}