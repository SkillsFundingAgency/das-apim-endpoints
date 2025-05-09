using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.Qualification;

public class AddQualificationDiscussionHistoryCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
{
    public string QualificationReference { get; set; } = string.Empty;
    public string? UserDisplayName { get; set; }
    public string? Notes { get; set; }
}
