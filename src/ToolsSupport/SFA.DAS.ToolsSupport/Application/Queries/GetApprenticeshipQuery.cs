using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Queries;
public class GetApprenticeshipQuery : IRequest<GetApprenticeshipQueryResult?>
{
    public long Id { get; set; }
}