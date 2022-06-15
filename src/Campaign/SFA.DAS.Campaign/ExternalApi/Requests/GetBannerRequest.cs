using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.ExternalApi.Requests
{
    public class GetBannerRequest : IGetApiRequest
    {
        public GetBannerRequest()
        {
        }

        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var getUrl = "entries?content_type=banner";

            return getUrl;
        }
    }
}