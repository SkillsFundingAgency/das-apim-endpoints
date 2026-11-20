using MediatR;

namespace SFA.DAS.Campaign.Application.Queries.Announcement
{
    public class GetAnnouncementBySlugQuery : IRequest<GetAnnouncementBySlugQueryResult>
    {
        public string Slug { get; set; }
    }
}
