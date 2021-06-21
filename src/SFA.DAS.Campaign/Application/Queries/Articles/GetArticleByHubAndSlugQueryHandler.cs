using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Application.Queries.Articles
{
    public class GetArticleByHubAndSlugQueryHandler : IRequestHandler<GetArticleByHubAndSlugQuery, GetArticleByHubAndSlugQueryResult>
    {
        private readonly IReliableCacheStorageService _reliableCacheStorageService;

        public GetArticleByHubAndSlugQueryHandler(
            IReliableCacheStorageService reliableCacheStorageService)
        {
            _reliableCacheStorageService = reliableCacheStorageService;
        }

        public async Task<GetArticleByHubAndSlugQueryResult> Handle(GetArticleByHubAndSlugQuery request, CancellationToken cancellationToken)
        {
            var article = await _reliableCacheStorageService.GetData<CmsContent>(new GetArticleEntriesRequest(request.Hub.ToTitleCase(), request.Slug), $"{request.Hub.ToTitleCase()}_{request.Slug}");

            var pageModel = new CmsPageModel().Build(article);
            
            return new GetArticleByHubAndSlugQueryResult
            {
                PageModel = pageModel
            };
        }
    }
}
