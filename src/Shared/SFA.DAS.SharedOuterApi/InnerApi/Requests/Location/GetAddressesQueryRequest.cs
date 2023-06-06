using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
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
