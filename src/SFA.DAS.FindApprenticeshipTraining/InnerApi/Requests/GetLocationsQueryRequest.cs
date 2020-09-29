using SFA.DAS.SharedOuterApi.Interfaces;
using System.Web;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
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