using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetUserOverview;

public class GetUserOverviewQuery : IRequest<GetUserOverviewQueryResult>
{
    public Guid UserId { get; set; }
}
