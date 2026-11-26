using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Rollover
{
    public class GetRolloverCandidatesQuery : IRequest<BaseMediatrResponse<GetRolloverCandidatesQueryResponse>>
    {
    }
}