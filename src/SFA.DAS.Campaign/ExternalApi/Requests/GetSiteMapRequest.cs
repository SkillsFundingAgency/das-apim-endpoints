using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.ExternalApi.Requests
{
    public class GetSiteMapRequest : IGetApiRequest
    {
        public GetSiteMapRequest()
        {
        }

        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var getUrl = $"entries?content_type=hub&include=2";

            return getUrl;
        }
    }
}