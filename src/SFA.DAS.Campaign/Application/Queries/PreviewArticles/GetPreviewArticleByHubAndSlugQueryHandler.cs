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
        private readonly IMediator _mediator;

        public GetPreviewArticleByHubAndSlugQueryHandler (IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> client, IMediator mediator)
        {
            _client = client;
            _mediator = mediator;
        }
    
        public async Task<GetPreviewArticleByHubAndSlugQueryResult> Handle(GetPreviewArticleByHubAndSlugQuery request, CancellationToken cancellationToken)
        {
            var article = _client.Get<CmsContent>(new GetArticleEntriesRequest(request.Hub.ToTitleCase(), request.Slug));
            var menu = _mediator.RetrieveMenu(cancellationToken);
            var banners = _mediator.RetrieveBanners(cancellationToken: cancellationToken);

            await Task.WhenAll(article, menu);

            var pageModel = new CmsPageModel().Build(article.Result, menu.Result.MainContent, banners.Result);
            
            return new GetPreviewArticleByHubAndSlugQueryResult
            {
                PageModel = pageModel
            };
        }
    }
}