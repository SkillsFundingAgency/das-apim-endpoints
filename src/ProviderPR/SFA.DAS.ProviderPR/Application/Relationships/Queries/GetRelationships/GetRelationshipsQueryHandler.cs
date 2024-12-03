using MediatR;
using RestEase;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationships;
public class GetRelationshipsQueryHandler(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient) : IRequestHandler<GetRelationshipsQuery, GetProviderRelationshipsResponse>
{
    public async Task<GetProviderRelationshipsResponse> Handle(GetRelationshipsQuery request, CancellationToken cancellationToken)
    {
        Response<GetProviderRelationshipsResponse> result =
            await _providerRelationshipsApiRestClient.GetProviderRelationships(request.Ukprn, request.Request, cancellationToken);

        return result.GetContent();
    }
}
