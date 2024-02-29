using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Application.Queries.PreviewLandingPage
{
    public class GetPreviewLandingPageQueryHandler : IRequestHandler<GetPreviewLandingPageQuery, GetPreviewLandingPageQueryResult>
    {
        private readonly IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> _client;
        private readonly IMediator _mediator;

        public GetPreviewLandingPageQueryHandler (IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> client, IMediator mediator)
        {
            _client = client;
            _mediator = mediator;
        }
        
        public async Task<GetPreviewLandingPageQueryResult> Handle(GetPreviewLandingPageQuery request, CancellationToken cancellationToken)
        {
            var landingPage = _client.Get<CmsContent>(new GetLandingPageRequest(request.Hub.ToTitleCase(), request.Slug));
            var menu = _mediator.RetrieveMenu(cancellationToken);
            var banners = _mediator.RetrieveBanners(cancellationToken: cancellationToken);


            await Task.WhenAll(landingPage, menu, banners);
            var pageModel = new LandingPageModel().Build(landingPage.Result, menu.Result.MainContent, banners.Result);

            return new GetPreviewLandingPageQueryResult()
            {
                PageModel = pageModel
            };
        }
    }
}