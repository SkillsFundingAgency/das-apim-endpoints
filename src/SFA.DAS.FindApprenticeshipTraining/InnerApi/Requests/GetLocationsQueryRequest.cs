using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetLocationsQueryRequest :IGetApiRequest
    {
        private readonly string _query;

        public GetLocationsQueryRequest(string query)
        {
            _query = query;
        }

        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}api/locations/search?query={_query}";
    }
}