using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Campaign.ExternalApi.Requests
{
    public class GetHomepageRequest : IGetApiRequest
    {
        public string GetUrl => "entries?content_type=homepage&include=3";
    }
}
