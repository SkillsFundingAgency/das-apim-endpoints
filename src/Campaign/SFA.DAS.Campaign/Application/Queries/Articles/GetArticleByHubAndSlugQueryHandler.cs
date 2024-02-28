using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Application.Queries.Articles
{
    public class GetArticleByHubAndSlugQueryHandler : IRequestHandler<GetArticleByHubAndSlugQuery, GetArticleByHubAndSlugQueryResult>
    {
        private readonly IReliableCacheStorageService _reliableCacheStorageService;
        private readonly IMediator _mediator;
        private readonly IContentService _contentService;

        public GetArticleByHubAndSlugQueryHandler(
            IReliableCacheStorageService reliableCacheStorageService, IMediator mediator, IContentService contentService)
        {
            _reliableCacheStorageService = reliableCacheStorageService;
            _mediator = mediator;
            _contentService = contentService;
        }

        public async Task<GetArticleByHubAndSlugQueryResult> Handle(GetArticleByHubAndSlugQuery request, CancellationToken cancellationToken)
        {
            var article = _reliableCacheStorageService.GetData<CmsContent>(new GetArticleEntriesRequest(request.Hub.ToTitleCase(), request.Slug), $"{request.Hub.ToTitleCase()}_{request.Slug}_article", _contentService.HasContent);
            var menu = _mediator.RetrieveMenu(cancellationToken);
            var banners = _mediator.RetrieveBanners(cancellationToken);

            await Task.WhenAll(article, menu, banners);

            var pageModel = new CmsPageModel().Build(article.Result, menu.Result.MainContent, banners.Result);
            
            return new GetArticleByHubAndSlugQueryResult
            {
                PageModel = pageModel
            };
        }

        
    }
}
 