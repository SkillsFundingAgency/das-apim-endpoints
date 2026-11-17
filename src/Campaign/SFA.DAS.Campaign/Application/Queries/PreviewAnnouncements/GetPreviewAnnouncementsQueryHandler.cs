using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Application.Queries.PreviewAnnouncements
{
    public class GetPreviewAnnouncementsQueryHandler : IRequestHandler<GetPreviewAnnouncementsQuery, GetPreviewAnnouncementsQueryResult>
    {
        private readonly IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> _client;

        public GetPreviewAnnouncementsQueryHandler(IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> client)
        {
            _client = client;
        }

        public async Task<GetPreviewAnnouncementsQueryResult> Handle(GetPreviewAnnouncementsQuery request, CancellationToken cancellationToken)
        {
            var announcements = await _client.Get<CmsContent>(new GetAnnouncementsRequest());

            return new GetPreviewAnnouncementsQueryResult
            {
                PageModel = new AnnouncementsPageModel().Build(announcements)
            };
        }
    }
}
