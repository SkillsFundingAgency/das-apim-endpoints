using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Rollover
{
    public class GetRolloverWorkflowCandidatesQuery : IRequest<BaseMediatrResponse<GetRolloverWorkflowCandidatesQueryResponse>>
    {
    }
}