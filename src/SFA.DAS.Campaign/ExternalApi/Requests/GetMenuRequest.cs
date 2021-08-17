using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.ExternalApi.Requests
{
    public class GetMenuRequest : IGetApiRequest
    {
        private readonly string _menuType;

        public GetMenuRequest(string menuType)
        {
            _menuType = menuType;
        }

        public string GetUrl => BuildUrl();

        private string BuildUrl()
        {
            var getUrl = $"entries?content_type=navigationMenu&fields.type={_menuType}";

            return getUrl;
        }
    }
}