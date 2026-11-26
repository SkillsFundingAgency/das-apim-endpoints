using SFA.DAS.Apim.Shared.Interfaces;
using System.Web;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;

[Obsolete("GetLocationByFullPostcodeRequestV2 should be used instead, note it returns a slightly different response to this request")]
public class GetLocationByFullPostcodeRequest(string fullPostcode) : IGetApiRequest
{
    public string GetUrl => $"api/postcodes?postcode={HttpUtility.UrlEncode(fullPostcode)}";
}