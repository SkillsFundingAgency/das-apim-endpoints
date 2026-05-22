using MediatR;
namespace SFA.DAS.Aodp.Application.Commands.Rollover;

public record UpdateRolloverWorkflowCandidatesAfterP1ChecksCommand : IRequest<BaseMediatrResponse<EmptyResponse>>
{
}
