using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Application.Queries.Menu
{
    public class GetMenuQueryHandler : IRequestHandler<GetMenuQuery, GetMenuQueryResult>
    {
        private readonly IReliableCacheStorageService _reliableCacheStorageService;
        private readonly IContentService _contentService;

        public GetMenuQueryHandler(IReliableCacheStorageService reliableCacheStorageService, IContentService contentService)
        {
            _reliableCacheStorageService = reliableCacheStorageService;
            _contentService = contentService;
        }

        public async Task<GetMenuQueryResult> Handle(GetMenuQuery request, CancellationToken cancellationToken)
        {
            var menu = await _reliableCacheStorageService.GetData<CmsContent>(new GetMenuRequest(request.MenuType), $"{request.MenuType}_menu", _contentService.HasContent);

            var pageModel = new MenuPageModel().Build(menu);

            return new GetMenuQueryResult
            {
                PageModel = pageModel
            };
        }
    }
}
