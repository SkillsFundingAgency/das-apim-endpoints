using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.ExternalApi.Requests
{
    public class GetHubEntriesRequest : IGetApiRequest
    {
        private readonly string _hubType;

        public GetHubEntriesRequest(string hubType)
        {
            _hubType = hubType;
        }

        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var getUrl = $"entries?content_type=hub&include=2";

            if (!string.IsNullOrEmpty(_hubType))
            {
                getUrl += $"&fields.hubType={_hubType}";
            }
            


            return getUrl;
        }
    }
}