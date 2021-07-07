using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Application.Queries.PreviewArticles
{
    public class GetPreviewArticleByHubAndSlugQueryHandler : IRequestHandler<GetPreviewArticleByHubAndSlugQuery, GetPreviewArticleByHubAndSlugQueryResult>
    {
        private readonly IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> _client;

        public GetPreviewArticleByHubAndSlugQueryHandler (IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> client)
        {
            _client = client;
        }
    
        public async Task<GetPreviewArticleByHubAndSlugQueryResult> Handle(GetPreviewArticleByHubAndSlugQuery request, CancellationToken cancellationToken)
        {
            var article = await _client.Get<CmsContent>(new GetArticleEntriesRequest(request.Hub.ToTitleCase(), request.Slug));
            
            if (article.Total == 0)
            {
                article = await _client.Get<CmsContent>(new GetLandingPageRequest(request.Hub.ToTitleCase(), request.Slug));
            }
            
            var pageModel = new CmsPageModel().Build(article);
            
            return new GetPreviewArticleByHubAndSlugQueryResult
            {
                PageModel = pageModel
            };
        }
    }
}