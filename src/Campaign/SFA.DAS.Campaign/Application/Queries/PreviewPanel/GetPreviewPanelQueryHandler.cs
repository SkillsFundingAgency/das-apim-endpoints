﻿using MediatR;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.Campaign.Models;
using SFA.DAS.Campaign.Extensions;

namespace SFA.DAS.Campaign.Application.Queries.PreviewPanel
{
    public class GetPreviewPanelQueryHandler : IRequestHandler<GetPreviewPanelQuery, GetPreviewPanelQueryResult>
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
            var panel = await _client.Get<CmsContent>(new GetPanelRequest(request.Slug));
            
            var pageModel = new PanelModel().Build(panel);

            return new GetPreviewPanelQueryResult
            {
                PanelModel = pageModel
            };
        }
    }
}
