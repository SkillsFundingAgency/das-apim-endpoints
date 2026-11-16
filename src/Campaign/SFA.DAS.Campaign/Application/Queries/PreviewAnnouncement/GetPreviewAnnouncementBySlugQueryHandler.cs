using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Application.Queries.PreviewAnnouncement
{
    public class GetPreviewAnnouncementBySlugQueryHandler : IRequestHandler<GetPreviewAnnouncementBySlugQuery, GetPreviewAnnouncementBySlugQueryResult>
    {
        private readonly IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> _client;

        public GetPreviewAnnouncementBySlugQueryHandler(IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> client)
        {
            _client = client;
        }

        public async Task<GetPreviewAnnouncementBySlugQueryResult> Handle(GetPreviewAnnouncementBySlugQuery request, CancellationToken cancellationToken)
        {
            var announcement = await _client.Get<CmsContent>(new GetAnnouncementBySlugRequest(request.Slug));

            return new GetPreviewAnnouncementBySlugQueryResult
            {
                PageModel = new AnnouncementModel().Build(announcement)
            };
        }
    }
}
