using MediatR;

namespace SFA.DAS.Aodp.Application.Commands.Rollover
{
    public class RemovePreviousWorkflowCandidatesCommand : IRequest<BaseMediatrResponse<RemovePreviousWorkflowCandidatesCommandResponse>>
    {
        public Guid RolloverWorkflowRunId { get; set; }
        public List<Guid> CandidateIds { get; set; } = new();
    }
}
