using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.ProviderPermissions.Queries;

public class GetHasPermissionQueryHandler(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> apiClient)
    : IRequestHandler<GetHasPermissionQuery, bool>
{
    public async Task<bool> Handle(GetHasPermissionQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.GetWithResponseCode<GetHasPermissionResponse>(new GetHasPermissionRequest(request.Ukprn, request.AccountLegalEntityId, request.Operation));

        response.EnsureSuccessStatusCode();

        return response.Body.HasPermission;
    }
}