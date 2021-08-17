using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.ExternalApi.Requests
{
    public class GetSiteMapRequest : IGetApiRequest
    {
        private readonly string _contentType;

        public GetSiteMapRequest(string contentType)
        {
            _contentType = contentType;
        }

        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var getUrl = $"entries?content_type={_contentType}&include=2";

            return getUrl;
        }
    }
}