using SFA.DAS.Apim.Shared.Interfaces;
using System.Web;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;

public class GetLocationsQueryRequest(string query) : IGetApiRequest
{
    public string GetUrl => $"api/search?query={HttpUtility.UrlEncode(query)}";
}