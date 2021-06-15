using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.Interfaces;

namespace SFA.DAS.Campaign.Application.Queries.Articles
{
    public class GetArticleByHubAndSlugQueryHandler : IRequestHandler<GetArticleByHubAndSlugQuery, GetArticleByHubAndSlugQueryResult>
    {
        private readonly IContentfulService _contentfulService;

        public GetArticleByHubAndSlugQueryHandler(IContentfulService contentfulService)
        {
            _contentfulService = contentfulService;
        }

        public async Task<GetArticleByHubAndSlugQueryResult> Handle(GetArticleByHubAndSlugQuery request, CancellationToken cancellationToken)
        {
            var article = await _contentfulService.GetArticleForAsync(request.Hub.ToTitleCase(), request.Slug, cancellationToken);

            return new GetArticleByHubAndSlugQueryResult
            {
                Article = article
            };
        }
    }
}
