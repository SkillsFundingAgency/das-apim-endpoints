using MediatR;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetChallenge;

public class GetChallengeQuery : IRequest<GetChallengeQueryResult>
{
    public long AccountId { get; set; }
}
