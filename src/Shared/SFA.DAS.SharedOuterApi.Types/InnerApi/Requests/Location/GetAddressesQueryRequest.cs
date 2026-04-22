using System.Web;

using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location
{
    public class GetAddressesQueryRequest : IGetApiRequest
    {
        public string Query { get; }
        public double MinMatch { get; }

        public GetAddressesQueryRequest(string query, double minMatch)
        {
            Query = query;
            MinMatch = minMatch;
        }

        public string GetUrl => $"api/addresses?query={HttpUtility.UrlEncode(Query)}&minMatch={MinMatch}";
    }
}
