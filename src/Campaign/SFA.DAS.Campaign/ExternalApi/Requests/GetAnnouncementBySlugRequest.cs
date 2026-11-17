using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Campaign.ExternalApi.Requests
{
    public class GetAnnouncementBySlugRequest : IGetApiRequest
    {
        private readonly string _slug;

        public GetAnnouncementBySlugRequest(string slug)
        {
            _slug = slug;
        }

        public string GetUrl => $"entries?content_type=announcements&include=2&fields.slug={_slug}";
    }
}
