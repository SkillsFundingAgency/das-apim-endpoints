using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Application.Queries.Hub;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Application.Queries.LandingPage
{
    public class GetLandingPageQueryHandler : IRequestHandler<GetLandingPageQuery, GetLandingPageQueryResult>
    {
        private readonly IReliableCacheStorageService _reliableCacheStorageService;
        private readonly IMediator _mediator;
        private readonly IContentService _contentService;

        public GetLandingPageQueryHandler(
            IReliableCacheStorageService reliableCacheStorageService, IMediator mediator, IContentService contentService)
        {
            _reliableCacheStorageService = reliableCacheStorageService;
            _mediator = mediator;
            _contentService = contentService;
        }

        public async Task<GetLandingPageQueryResult> Handle(GetLandingPageQuery request, CancellationToken cancellationToken)
        {
            var landingPage = _reliableCacheStorageService.GetData<CmsContent>(
                new GetLandingPageRequest(request.Hub.ToTitleCase(), request.Slug), 
                $"{request.Hub.ToTitleCase()}_{request.Slug}_landingPage", 
                _contentService.HasContent);
            var menu = _mediator.RetrieveMenu(cancellationToken);
            var banners = _mediator.RetrieveBanners(cancellationToken);

            await Task.WhenAll(landingPage, menu, banners);

            var pageModel = new LandingPageModel().Build(landingPage.Result, menu.Result.MainContent, banners.Result);

            return new GetLandingPageQueryResult
            {
                PageModel = pageModel
            };
        }
    }
}
