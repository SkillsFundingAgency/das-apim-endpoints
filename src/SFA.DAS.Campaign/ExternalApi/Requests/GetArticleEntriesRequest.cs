using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.ExternalApi.Requests
{
    public class GetArticleEntriesRequest : IGetApiRequest
    {
        private readonly string _entryId;
        private readonly string _hubType;
        private readonly string _slug;

        public GetArticleEntriesRequest(string hubType, string slug)
        {
            _hubType = hubType;
            _slug = slug;
        }

        public GetArticleEntriesRequest(string entryId)
        {
            _entryId = entryId;
        }

        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var getUrl = $"entries?content_type=article";
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