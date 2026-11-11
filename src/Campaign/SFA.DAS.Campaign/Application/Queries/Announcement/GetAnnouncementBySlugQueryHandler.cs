using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Application.Queries.Announcement
{
    public class GetAnnouncementBySlugQueryHandler : IRequestHandler<GetAnnouncementBySlugQuery, GetAnnouncementBySlugQueryResult>
    {
        private readonly IReliableCacheStorageService _reliableCacheStorageService;
        private readonly IContentService _contentService;

        public GetAnnouncementBySlugQueryHandler(IReliableCacheStorageService reliableCacheStorageService, IContentService contentService)
        {
            _reliableCacheStorageService = reliableCacheStorageService;
            _contentService = contentService;
        }

        public async Task<GetAnnouncementBySlugQueryResult> Handle(GetAnnouncementBySlugQuery request, CancellationToken cancellationToken)
        {
            var announcement = await _reliableCacheStorageService.GetData<CmsContent>(
                new GetAnnouncementBySlugRequest(request.Slug),
                $"{request.Slug}_announcement",
                _contentService.HasContent);

            return new GetAnnouncementBySlugQueryResult
            {
                PageModel = new AnnouncementModel().Build(announcement)
            };
        }
    }
}
