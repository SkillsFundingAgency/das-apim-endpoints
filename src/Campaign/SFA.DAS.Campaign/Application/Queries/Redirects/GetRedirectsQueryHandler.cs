using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Application.Queries.Redirects
{
    public class GetRedirectsQueryHandler : IRequestHandler<GetRedirectsQuery, GetRedirectsQueryResult>
    {
        public const string CacheKey = "Redirects";

        private readonly IReliableCacheStorageService _reliableCacheStorageService;
        private readonly IContentService _contentService;

        public GetRedirectsQueryHandler(IReliableCacheStorageService reliableCacheStorageService, IContentService contentService)
        {
            _reliableCacheStorageService = reliableCacheStorageService;
            _contentService = contentService;
        }

        public async Task<GetRedirectsQueryResult> Handle(GetRedirectsQuery request, CancellationToken cancellationToken)
        {
            var content = await _reliableCacheStorageService.GetData<CmsContent>(new GetRedirectsRequest(), CacheKey, _contentService.HasContent);

            return new GetRedirectsQueryResult
            {
                Redirects = RedirectModel.BuildFrom(content)
            };
        }
    }
}
