using MediatR;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Application.Queries.PreviewPanel
{
    public class GetPreviewPanelQueryHandler : IRequestHandler<GetPreviewPanelQuery, GetPreviewPanelQueryResult>
    {
        private readonly IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> _client;

        public GetPreviewPanelQueryHandler(IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> client)
        {
            _client = client;
        }

        public async Task<GetPreviewPanelQueryResult> Handle(GetPreviewPanelQuery request, CancellationToken cancellationToken)
        {
            var panel = await _client.Get<CmsContent>(new GetPanelRequest(request.Id));
            
            var pageModel = new PanelModel().Build(panel);

            return new GetPreviewPanelQueryResult
            {
                PanelModel = pageModel
            };
        }
    }
}
