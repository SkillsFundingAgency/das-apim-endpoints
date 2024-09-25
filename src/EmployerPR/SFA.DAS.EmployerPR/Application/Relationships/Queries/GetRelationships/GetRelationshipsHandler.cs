using MediatR;
using SFA.DAS.EmployerPR.Infrastructure;

namespace SFA.DAS.EmployerPR.Application.Relationships.Queries.GetRelationships;
public class GetRelationshipsHandler(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient) : IRequestHandler<GetRelationshipsQuery, GetRelationshipsResponse>
{
    public async Task<GetRelationshipsResponse> Handle(GetRelationshipsQuery query, CancellationToken cancellationToken)
    {
        var response =
            await _providerRelationshipsApiRestClient.GetRelationships(query.Ukprn, query.AccountLegalEntityId, cancellationToken);

        return response;
    }
}