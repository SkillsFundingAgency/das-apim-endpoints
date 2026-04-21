using SFA.DAS.Apim.Shared.Interfaces;
using System.Web;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;

public class GetLocationByOutcodeAndDistrictRequest(string outcode) : IGetApiRequest
{
    public string GetUrl => $"api/postcodes/outcode?outcode={HttpUtility.UrlEncode(outcode)}";
}