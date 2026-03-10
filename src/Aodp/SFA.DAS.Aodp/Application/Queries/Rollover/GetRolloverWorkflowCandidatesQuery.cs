using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Rollover;

public record GetRolloverWorkflowCandidatesQuery(int? Skip = 0, int? Take = 0) : IRequest<BaseMediatrResponse<GetRolloverWorkflowCandidatesQueryResponse>>;

