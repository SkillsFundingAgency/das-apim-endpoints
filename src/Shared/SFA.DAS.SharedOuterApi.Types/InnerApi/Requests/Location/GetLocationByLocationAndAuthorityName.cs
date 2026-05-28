using SFA.DAS.Apim.Shared.Interfaces;
using System.Web;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;

public class GetLocationByLocationAndAuthorityName(string locationName, string authorityName) : IGetApiRequest
{
    public string GetUrl => $"api/locations?locationName={HttpUtility.UrlEncode(locationName)}&authorityName={HttpUtility.UrlEncode(authorityName)}";
}