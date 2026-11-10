using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Application.Queries.PreviewHomepage
{
    public class GetPreviewHomepageQueryHandler : IRequestHandler<GetPreviewHomepageQuery, GetPreviewHomepageQueryResult>
    {
        private readonly IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> _client;

        public GetPreviewHomepageQueryHandler(IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> client)
        {
            _client = client;
        }

        public async Task<GetPreviewHomepageQueryResult> Handle(GetPreviewHomepageQuery request, CancellationToken cancellationToken)
        {
            var homepage = await _client.Get<CmsContent>(new GetHomepageRequest());

            return new GetPreviewHomepageQueryResult
            {
                PageModel = new HomepageModel().Build(homepage)
            };
        }
    }
}
