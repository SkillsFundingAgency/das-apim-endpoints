using MediatR;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Application.Queries.Panel
{
    public class GetPanelQueryResult : IRequest<GetPanelQuery>
    {
        public PanelModel Panel { get; set; }
    }
}
