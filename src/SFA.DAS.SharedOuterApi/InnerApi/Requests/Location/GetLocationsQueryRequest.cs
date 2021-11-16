using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetLocationsQueryRequest :IGetApiRequest
    {
        private readonly string _query;

        public GetLocationsQueryRequest(string query)
        {
            _query = query;
        }

        public string GetUrl => $"api/search?query={HttpUtility.UrlEncode(_query)}";
    }
}