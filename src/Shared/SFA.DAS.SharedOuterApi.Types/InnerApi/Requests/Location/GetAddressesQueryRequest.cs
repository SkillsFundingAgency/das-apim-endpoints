using SFA.DAS.Apim.Shared.Interfaces;
using System.Web;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;

public class GetAddressesQueryRequest(string query, double minMatch) : IGetApiRequest
{
    public string Query { get; } = query;
    public double MinMatch { get; } = minMatch;

    public string GetUrl => $"api/addresses?query={HttpUtility.UrlEncode(Query)}&minMatch={MinMatch}";
}