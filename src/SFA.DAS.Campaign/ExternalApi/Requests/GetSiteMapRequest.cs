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
            var getUrl = $"entries?select=sys.id,sys.contentType,fields.slug,fields.title,fields.hubType&content_type={_contentType}";

            return getUrl;
        }
    }
}