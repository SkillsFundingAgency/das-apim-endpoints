using RestEase;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.Infrastructure;

public interface IProviderRelationshipsApiRestClient
{
    [Get("/health")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> GetHealth(CancellationToken cancellationToken);

    [Get("relationships/providers/{ukprn}")]
    Task<GetProviderRelationshipsResponse> GetProviderRelationships([Path] long ukprn, [RawQueryString] string queryString, CancellationToken cancellationToken);

    [Get("relationships")]
    [AllowAnyStatusCode]
    Task<Response<GetRelationshipResponse>> GetRelationship([Query] long ukprn, [Query] long accountLegalEntityId, CancellationToken cancellationToken);
}
