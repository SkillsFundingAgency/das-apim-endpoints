using MediatR;

namespace SFA.DAS.Campaign.Application.Queries.PreviewAnnouncement
{
    public class GetPreviewAnnouncementBySlugQuery : IRequest<GetPreviewAnnouncementBySlugQueryResult>
    {
        public string Slug { get; set; }
    }
}
