using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Application.Queries.Banner
{
    public class GetBannerQueryHandler : IRequestHandler<GetBannerQuery, GetBannerQueryResult>
    {
        private readonly IReliableCacheStorageService _reliableCacheStorageService;
        private readonly IContentService _contentService;

        public GetBannerQueryHandler(IReliableCacheStorageService reliableCacheStorageService, IContentService contentService)
        {
            _reliableCacheStorageService = reliableCacheStorageService;
            _contentService = contentService;
        }

        public async Task<GetBannerQueryResult> Handle(GetBannerQuery request, CancellationToken cancellationToken)
        {
            var banner = await _reliableCacheStorageService.GetData<CmsContent>(new GetBannerRequest(), $"Banners", _contentService.HasContent);

            var pageModel = new BannerPageModel().Build(banner);

            return new GetBannerQueryResult
            {
                PageModel = pageModel
            };
        }
    }
}
