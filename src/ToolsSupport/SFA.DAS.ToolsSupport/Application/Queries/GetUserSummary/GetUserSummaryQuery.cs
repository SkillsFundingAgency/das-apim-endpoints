using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetUserSummary;

public class GetUserSummaryQuery : IRequest<GetUserSummaryQueryResult>
{
    public Guid UserId { get; set; }
}
