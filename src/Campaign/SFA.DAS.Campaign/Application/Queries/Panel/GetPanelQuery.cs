using MediatR;

namespace SFA.DAS.Campaign.Application.Queries.Panel
{
    public class GetPanelQuery : IRequest<GetPanelQueryResult>
    {
        public int Id { get; set; }
    }
}
