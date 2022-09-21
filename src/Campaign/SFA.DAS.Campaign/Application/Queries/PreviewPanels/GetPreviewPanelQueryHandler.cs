using MediatR;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.Campaign.Models;
using SFA.DAS.Campaign.Extensions;

namespace SFA.DAS.Campaign.Application.Queries.PreviewPanels
{
    public class GetPreviewPanelQueryHandler: IRequestHandler<GetPreviewPanelQuery, GetPreviewPanelQueryResult>
    {
        private readonly IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> _client;
        private readonly IMediator _mediator;

        public GetPreviewPanelQueryHandler(IContentfulPreviewApiClient<ContentfulPreviewApiConfiguration> client, IMediator mediator)
        {
            _client = client;
            _mediator = mediator;
        }

        public async Task<GetPreviewPanelQueryResult> Handle(GetPreviewPanelQuery request, CancellationToken cancellationToken)
        {
            var panel = _client.Get<CmsContent>(new GetPanelRequest(request.Slug));
            var menu = _mediator.RetrieveMenu(cancellationToken);

            await Task.WhenAll(panel, menu);

            var pageModel = new PanelPageModel().Build(panel.Result, menu.Result.MainContent);

            return new GetPreviewPanelQueryResult
            {
                PanelModel = pageModel
            };
        }
}
}
