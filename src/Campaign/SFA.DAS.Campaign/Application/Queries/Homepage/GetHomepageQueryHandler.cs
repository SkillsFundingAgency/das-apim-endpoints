using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Application.Queries.Homepage
{
    public class GetHomepageQueryHandler : IRequestHandler<GetHomepageQuery, GetHomepageQueryResult>
    {
        public const string CacheKey = "Homepage";

        private readonly IReliableCacheStorageService _reliableCacheStorageService;
        private readonly IContentService _contentService;

        public GetHomepageQueryHandler(IReliableCacheStorageService reliableCacheStorageService, IContentService contentService)
        {
            _reliableCacheStorageService = reliableCacheStorageService;
            _contentService = contentService;
        }

        public async Task<GetHomepageQueryResult> Handle(GetHomepageQuery request, CancellationToken cancellationToken)
        {
            var homepage = await _reliableCacheStorageService.GetData<CmsContent>(
                new GetHomepageRequest(),
                CacheKey,
                _contentService.HasContent);

            return new GetHomepageQueryResult
            {
                PageModel = new HomepageModel().Build(homepage)
            };
        }
    }
}
