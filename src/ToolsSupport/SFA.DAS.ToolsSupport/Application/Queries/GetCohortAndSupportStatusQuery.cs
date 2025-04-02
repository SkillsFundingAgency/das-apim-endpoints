using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Queries;
public class GetCohortAndSupportStatusQuery : IRequest<GetCohortAndSupportStatusQueryResult?>
{
    public long CohortId { get; set; }
}