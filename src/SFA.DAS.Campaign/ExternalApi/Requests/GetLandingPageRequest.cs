using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.ExternalApi.Requests
{
    public class GetLandingPageRequest : IGetApiRequest
    {
        private readonly string _entryId;
        private readonly string _hubType;
        private readonly string _slug;

        public GetLandingPageRequest (string hubType, string slug)
        {
            _hubType = hubType;
            _slug = slug;
        }

        public GetLandingPageRequest (string entryId)
        {
            _entryId = entryId;
        }

        public string GetUrl => BuildUrl();
        
        private string BuildUrl()
        {
            var getUrl = $"entries?content_type=landingPage&include=2";
            if (!string.IsNullOrEmpty(_hubType))
            {
                getUrl += $"&fields.hubType={_hubType}";
            }
            if (!string.IsNullOrEmpty(_slug))
            {
                getUrl += $"&fields.slug={_slug}";
            }

            if (!string.IsNullOrEmpty(_entryId))
            {
                getUrl += $"&sys.id={_entryId}";
            }
            
            return getUrl;
        }
    }
}