using MediatR;
using RestEase;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationship;

public class GetRelationshipQueryHandler(IProviderRelationshipsApiRestClient _providerRelationshipsApiClient) : IRequestHandler<GetRelationshipQuery, GetRelationshipResponse>
{
    public async Task<GetRelationshipResponse> Handle(GetRelationshipQuery request, CancellationToken cancellationToken)
    {
        Response<GetRelationshipResponse> relationshipResponse = await _providerRelationshipsApiClient.GetRelationship(request.Ukprn, request.AccountLegalEntityId, cancellationToken);

        return relationshipResponse.GetContent();
    }
}
