using MediatR;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Application.Queries.PreviewPanel
{
    public class GetPreviewPanelQueryResult : IRequest<GetPreviewPanelQuery>
    {
        public PanelModel PanelModel { get; set; }
    }
}
