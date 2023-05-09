using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.ExternalApi.Requests
{
    public class GetPanelRequest : IGetApiRequest
    {
        private readonly int _panelId;

        public GetPanelRequest(int id)
        {
            _panelId = id;
        }

        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var getUrl = $"entries?content_type=panel&fields.id={_panelId}";

            return getUrl;
        }
    }
}
