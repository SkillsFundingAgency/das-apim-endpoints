using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetAddressesQueryRequest : IGetApiRequest
    {
        private readonly string _query;
        private readonly double _minMatch;

        public GetAddressesQueryRequest(string query, double minMatch)
        {
            _query = query;
            _minMatch = minMatch;
        }
        
        public string GetUrl => $"api/addresses?query={HttpUtility.UrlEncode(_query)}&minMatch={_minMatch}";
    }
}
