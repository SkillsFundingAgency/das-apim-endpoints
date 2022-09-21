using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.ExternalApi.Requests
{
    public class GetPanelRequest : IGetApiRequest
    {
        private readonly string _slug;

        public GetPanelRequest(string slug)
        {
            _slug = slug;
        }

        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var getUrl = $"entries?content_type=panel&fields.slug={_slug}";

            return getUrl;
        }
    }
}
