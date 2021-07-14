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

        public GetPreviewHubQueryHandler(IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> client)
        {
            _client = client;
        }

        public async Task<GetPreviewHubQueryResult> Handle(GetPreviewHubQuery request, CancellationToken cancellationToken)
        {
            var article = await _client.Get<CmsContent>(new GetHubEntriesRequest(request.Hub.ToTitleCase()));

            var pageModel = new HubPageModel().Build(article);

            return new GetPreviewHubQueryResult
            {
                PageModel = pageModel
            };
        }
    }
}
