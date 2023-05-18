using MediatR;

namespace SFA.DAS.Campaign.Application.Queries.PreviewPanel
{
    public class GetPreviewPanelQuery : IRequest<GetPreviewPanelQueryResult>
    {
        public int Id { get; set; }
    }
}
