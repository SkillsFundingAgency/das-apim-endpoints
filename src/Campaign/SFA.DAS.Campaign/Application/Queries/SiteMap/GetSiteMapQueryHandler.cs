using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Application.Queries.SiteMap
{
    public class GetSiteMapQueryHandler : IRequestHandler<GetSiteMapQuery, GetSiteMapQueryResult>
    {
        private readonly IReliableCacheStorageService _reliableCacheStorageService;
        private readonly IContentService _contentService;

        public GetSiteMapQueryHandler(IReliableCacheStorageService reliableCacheStorageService, IContentService contentService)
        {
            _reliableCacheStorageService = reliableCacheStorageService;
            _contentService = contentService;
        }

        public async Task<GetSiteMapQueryResult> Handle(GetSiteMapQuery request, CancellationToken cancellationToken)
        {
            var article = await _reliableCacheStorageService.GetData<CmsContent>(new GetSiteMapRequest(request.ContentType), $"SiteMap_{request.ContentType}", _contentService.HasContent);

            var pageModel = new SiteMapPageModel().Build(article);

            return new GetSiteMapQueryResult
            {
                MapModel = pageModel
            };
        }
    }
}
