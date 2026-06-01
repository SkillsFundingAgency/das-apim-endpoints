using MediatR;
namespace SFA.DAS.Aodp.Application.Queries.Rollover
{
    public class GetRolloverCandidatesForExportQuery : IRequest<BaseMediatrResponse<GetRolloverCandidatesForExportQueryResponse>>
    {
        public Guid RolloverWorkflowRunId { get; set; }
    }
}