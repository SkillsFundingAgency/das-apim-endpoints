using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerPR.Application.Queries.GetPermissions;

public class GetPermissionsHandler : IRequestHandler<GetPermissionsQuery, GetPermissionsResponse>
{
    private readonly IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> _apiClient;

    public GetPermissionsHandler(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task<GetPermissionsResponse> Handle(GetPermissionsQuery query, CancellationToken cancellationToken)
    {
        var response =
            await _apiClient.Get<GetPermissionsResponse>(new GetPermissionsRequest(query.Ukprn, query.PublicHashedId));

        return response;
    }
}