using MediatR;

namespace SFA.DAS.Campaign.Application.Queries.Menu
{
    public class GetMenuQuery : IRequest<GetMenuQueryResult>
    {
        public string MenuType { get; set; }
    }
}
