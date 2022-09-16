namespace SFA.DAS.Campaign.ExternalApi.Requests
{
    public class GetPanelRequest
    {
        private readonly string _title;

        public GetPanelRequest(string title)
        {
            _title = title;
        }

        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var getUrl = $"entries?content_type=panel?title={_title}";

            return getUrl;
        }
        //come back to this url
    }
}
