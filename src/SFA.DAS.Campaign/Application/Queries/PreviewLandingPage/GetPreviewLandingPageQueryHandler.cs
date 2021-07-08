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

namespace SFA.DAS.Campaign.Application.Queries.PreviewLandingPage
{
    public class GetPreviewLandingPageQueryHandler : IRequestHandler<GetPreviewLandingPageQuery, GetPreviewLandingPageQueryResult>
    {
        private readonly IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> _client;

        public GetPreviewLandingPageQueryHandler (IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> client)
        {
            _client = client;
        }
        
        public async Task<GetPreviewLandingPageQueryResult> Handle(GetPreviewLandingPageQuery request, CancellationToken cancellationToken)
        {
            var landingPage = await _client.Get<CmsContent>(new GetLandingPageRequest(request.Hub.ToTitleCase(), request.Slug));
            
            var pageModel = new LandingPageModel().Build(landingPage);

            return new GetPreviewLandingPageQueryResult()
            {
                PageModel = pageModel
            };
        }
    }
}