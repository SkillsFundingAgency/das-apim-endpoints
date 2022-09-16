using MediatR;
using SFA.DAS.Campaign.Application.Queries.Panel;

namespace SFA.DAS.Campaign.Application.Queries.PreviewPanels
{
    public class GetPreviewPanelQuery : IRequest<GetPreviewPanelQueryResult>
    {
        public string Title { get; set; }
    }
}
