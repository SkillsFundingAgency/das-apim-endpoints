using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Application.Queries.Menu;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Application.Queries.Banner
{
    public class GetBannerQueryHandler : IRequestHandler<GetBannerQuery, GetBannerQueryResult>
    {
        private readonly IReliableCacheStorageService _reliableCacheStorageService;

        public GetBannerQueryHandler(
            IReliableCacheStorageService reliableCacheStorageService)
        {
            _reliableCacheStorageService = reliableCacheStorageService;
        }

        public async Task<GetBannerQueryResult> Handle(GetBannerQuery request, CancellationToken cancellationToken)
        {
            var banner = await _reliableCacheStorageService.GetData<CmsContent>(new GetBannerRequest(), $"Banners");

            var pageModel = new BannerPageModel().Build(banner);

            return new GetBannerQueryResult
            {
                PageModel = pageModel
            };
        }
    }
}
