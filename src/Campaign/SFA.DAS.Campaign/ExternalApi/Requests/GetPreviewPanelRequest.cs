using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.ExternalApi.Requests
{
    public class GetPreviewPanelRequest : IGetApiRequest
    {
        private readonly string _title;

        public GetPreviewPanelRequest(string title)
        {
            _title = title;
        }

        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var getUrl = $"entries?content_type=previewpanel?entryId={_title}";

            return getUrl;
        }
    }
}
