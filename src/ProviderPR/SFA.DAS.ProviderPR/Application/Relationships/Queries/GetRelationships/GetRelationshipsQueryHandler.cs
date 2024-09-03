using MediatR;

namespace SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationships;
public class GetRelationshipsQueryHandler : IRequestHandler<GetRelationshipsQuery, GetRelationshipsQueryResult>
{
    public Task<GetRelationshipsQueryResult> Handle(GetRelationshipsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new GetRelationshipsQueryResult { Message = $"Invoked get relationship at {DateTime.UtcNow}" });
    }
}
