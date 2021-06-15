using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contentful.Core.Models;
using MediatR;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Application.Queries.Articles
{
    public class GetArticleByHubAndSlugQueryHandler : IRequestHandler<GetArticleByHubAndSlugQuery, GetArticleByHubAndSlugQueryResult>
    {
        //private readonly IContentfulService _contentfulService;
        private readonly IReliableCacheStorageService _reliableCacheStorageService;

        public GetArticleByHubAndSlugQueryHandler(
            //IContentfulService contentfulService, 
            IReliableCacheStorageService reliableCacheStorageService)
        {
          //  _contentfulService = contentfulService;
            _reliableCacheStorageService = reliableCacheStorageService;
        }

        public async Task<GetArticleByHubAndSlugQueryResult> Handle(GetArticleByHubAndSlugQuery request, CancellationToken cancellationToken)
        {
            //TODO
            var article = await _reliableCacheStorageService.GetData<CmsContent>(new GetArticleEntriesRequest(request.Hub.ToTitleCase(), request.Slug), $"{request.Hub.ToTitleCase()}_{request.Slug}");
            //var article = await _contentfulService.GetArticleForAsync(request.Hub.ToTitleCase(), request.Slug, cancellationToken);

            return new GetArticleByHubAndSlugQueryResult
            {
                Article = article
            };
        }
    }
}
