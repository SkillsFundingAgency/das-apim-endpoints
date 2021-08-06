using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Application.Queries.PreviewArticles;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Application.Queries.PreviewHub
{
    public class GetPreviewHubQueryHandler : IRequestHandler<GetPreviewHubQuery, GetPreviewHubQueryResult>
    {
        private readonly IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> _client;
        private readonly IMediator _mediator;

        public GetPreviewHubQueryHandler(IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> client, IMediator mediator)
        {
            _client = client;
            _mediator = mediator;
        }

        public async Task<GetPreviewHubQueryResult> Handle(GetPreviewHubQuery request, CancellationToken cancellationToken)
        {
            var article = _client.Get<CmsContent>(new GetHubEntriesRequest(request.Hub.ToTitleCase()));
            var menu = _mediator.RetrieveMenu(cancellationToken);
            var banners = _mediator.RetrieveBanners(cancellationToken: cancellationToken);

            await Task.WhenAll(article, menu, banners);

            var pageModel = new HubPageModel().Build(article.Result, menu.Result.MainContent, banners.Result);
           
            return new GetPreviewHubQueryResult
            {
                PageModel = pageModel
            };
        }
    }
}
