using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetPayeSchemeLevyDeclarations;

public class GetPayeSchemeLevyDeclarationsQuery : IRequest<GetPayeSchemeLevyDeclarationsResult>
{
    public long AccountId { get; set; }
    public string PayeRef { get; set; } = "";
}
