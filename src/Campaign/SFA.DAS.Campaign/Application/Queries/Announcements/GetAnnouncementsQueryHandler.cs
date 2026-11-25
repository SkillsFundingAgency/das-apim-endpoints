using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Application.Queries.Announcements
{
    public class GetAnnouncementsQueryHandler : IRequestHandler<GetAnnouncementsQuery, GetAnnouncementsQueryResult>
    {
        public const string CacheKey = "Announcements";

        private readonly IReliableCacheStorageService _reliableCacheStorageService;
        private readonly IContentService _contentService;

        public GetAnnouncementsQueryHandler(IReliableCacheStorageService reliableCacheStorageService, IContentService contentService)
        {
            _reliableCacheStorageService = reliableCacheStorageService;
            _contentService = contentService;
        }

        public async Task<GetAnnouncementsQueryResult> Handle(GetAnnouncementsQuery request, CancellationToken cancellationToken)
        {
            var announcements = await _reliableCacheStorageService.GetData<CmsContent>(
                new GetAnnouncementsRequest(),
                CacheKey,
                _contentService.HasContent);

            return new GetAnnouncementsQueryResult
            {
                PageModel = new AnnouncementsPageModel().Build(announcements)
            };
        }
    }
}
