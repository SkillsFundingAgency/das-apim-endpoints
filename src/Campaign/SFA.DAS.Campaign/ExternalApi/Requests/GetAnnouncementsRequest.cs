using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Campaign.ExternalApi.Requests
{
    public class GetAnnouncementsRequest : IGetApiRequest
    {
        public string GetUrl => "entries?content_type=announcements&include=1&order=-fields.date";
    }
}
